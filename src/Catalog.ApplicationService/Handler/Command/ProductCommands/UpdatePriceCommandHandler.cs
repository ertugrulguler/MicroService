using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApiContract.Response.Command.ProductCommands;
using Catalog.ApplicationService.Communicator.BackOffice;
using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand, ResponseBase<List<UpdatePriceControlResult>>>
    {
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;


        public UpdatePriceCommandHandler(IProductSellerRepository productSellerRepository, IProductRepository productRepository,
            IDbContextHandler dbContextHandler, IBackOfficeCommunicator backOfficeCommunicator)
        {
            _productSellerRepository = productSellerRepository;
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
            _backOfficeCommunicator = backOfficeCommunicator;
        }

        public async Task<ResponseBase<List<UpdatePriceControlResult>>> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<List<UpdatePriceControlResult>>();
            response.Data = new List<UpdatePriceControlResult>();
            int errorCount = 0;

            var productList = await _productRepository.FilterByAsync(p => request.Items.Select(p => p.Code).Contains(p.Code));
            foreach (var productPrice in request.Items)
            {
                try
                {
                    if (productPrice.StockCount < 0)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = productPrice.Code + ApplicationMessage.StockCountNotZero.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }
                    if (productPrice.SalePrice > productPrice.ListPrice)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.IsNotSalePrice.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }

                    var product = productList.FirstOrDefault(p => p.Code == productPrice.Code);
                    if (product == null)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidCatalogProductId.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }

                    var productDetail = await _productRepository.GetProductByProductSellerId(product.Id, request.SellerId);
                    if (productDetail == null)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidProductId.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }

                    var productSeller = productDetail.ProductSellers.FirstOrDefault();
                    if (productSeller == null)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidProductId.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }

                    //var salePriceDifference = productPrice.SalePrice - productSeller.SalePrice;
                    //var salePriceAddition = productPrice.SalePrice + productSeller.SalePrice;
                    if (productPrice.SalePrice <= 0 || productPrice.ListPrice <= 0)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidPrice.UserMessage(),
                            Item = productPrice
                        });
                        errorCount++;
                        continue;
                    }

                    if (productSeller.SalePrice == 0)
                    {
                        productSeller.SetProductPrice(productPrice.SalePrice, productPrice.ListPrice);
                        _productSellerRepository.Update(productSeller);
                        continue;
                    }
                    var percentDiff = Math.Abs((productPrice.SalePrice - productSeller.SalePrice) / productSeller.SalePrice) * 100;
                    if (percentDiff > 30)
                    {
                        var createProductRequest = new UpdatePriceAndInventoryApproveRequest();
                        createProductRequest.SellerId = request.SellerId;
                        createProductRequest.Products = new List<CreateProduct>();

                        var createProduct = new CreateProduct
                        {
                            Name = product.Name,
                            DisplayName = product.DisplayName,
                            Description = product.Description,
                            Code = product.Code,
                            BrandId = product.BrandId,
                            GroupCode = productDetail.ProductGroups.FirstOrDefault()?.GroupCode,
                            StockCount = productSeller.StockCount,
                            StockCode = productSeller.StockCode,
                            ListPrice = productPrice.ListPrice,
                            SalePrice = productPrice.SalePrice,
                            InstallmentCount = productSeller.InstallmentCount,
                            CategoryId = productDetail.ProductCategories.FirstOrDefault().CategoryId,
                            Attributes = productDetail.ProductAttributes.Select(x => new CreateProductAttribute()
                            {
                                AttributeId = x.AttributeId,
                                AttributeValueId = x.AttributeValueId
                            }).ToList(),
                            Images = productDetail.ProductImages.Select(p => new CreateProductImage() { ImageUrl = p.Url }).ToList(),
                            Deliveries = productDetail.ProductDeliveries.Select(p => new CreateProductDelivery()
                            {
                                DeliveryId = p.DeliveryId,
                                DeliveryType = p.DeliveryType,
                                CityList = productDetail.ProductDeliveries.Where(y => y.DeliveryType == p.DeliveryType).Select(
                                    h => h.CityId.HasValue ? h.CityId.Value : Guid.Empty).Distinct().ToList()
                            }).ToList()
                        };

                        createProductRequest.Products.Add(createProduct);

                        var result = await _backOfficeCommunicator.UpdatePriceAndInventoryApprove(createProductRequest);
                        if (result.Success)
                        {
                            response.Data.Add(new UpdatePriceControlResult()
                            {
                                Error = ApplicationMessage.BackOfficeApprove.UserMessage(),
                                Item = productPrice
                            });
                            errorCount++;
                        }
                    }
                    else
                    {
                        productSeller.SetProductPrice(productPrice.SalePrice, productPrice.ListPrice);
                        _productSellerRepository.Update(productSeller);
                    }
                }
                catch (Exception ex)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = ApplicationMessage.UnhandledError.UserMessage(),
                        Item = productPrice
                    });
                    errorCount++;
                }
            }
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<List<UpdatePriceControlResult>>()
            {
                Data = response.Data,
                Success = errorCount > 0 ? false : true
            };


        }
    }
}