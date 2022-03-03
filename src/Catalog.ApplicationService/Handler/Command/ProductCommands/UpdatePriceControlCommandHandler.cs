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
    public class UpdatePriceControlCommandHandler : IRequestHandler<UpdatePriceControlCommand, ResponseBase<List<UpdatePriceControlResult>>>
    {
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IBackOfficeCommunicator _backOfficeCommunicator;


        public UpdatePriceControlCommandHandler(IProductSellerRepository productSellerRepository, IProductRepository productRepository,
            IDbContextHandler dbContextHandler, IBackOfficeCommunicator backOfficeCommunicator)
        {
            _productSellerRepository = productSellerRepository;
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
            _backOfficeCommunicator = backOfficeCommunicator;
        }

        public async Task<ResponseBase<List<UpdatePriceControlResult>>> Handle(UpdatePriceControlCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<List<UpdatePriceControlResult>>();
            response.Data = new List<UpdatePriceControlResult>();
            int errorCount = 0;

            foreach (var priceAndInventory in request.Items)
            {
                if (priceAndInventory.StockCount < 0)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = priceAndInventory.Code + ApplicationMessage.StockCountNotZero.UserMessage(),
                        Item = priceAndInventory
                    });
                    errorCount++;
                    continue;
                }
                if (priceAndInventory.SalePrice > priceAndInventory.ListPrice)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = ApplicationMessage.IsNotSalePrice.UserMessage(),
                        Item = priceAndInventory
                    });
                    errorCount++;
                    continue;
                }

                var product = await _productRepository.FindByAsync(p => p.Code == priceAndInventory.Code);
                if (product == null)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = ApplicationMessage.InvalidCatalogProductId.UserMessage(),
                        Item = priceAndInventory
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
                        Item = priceAndInventory
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
                        Item = priceAndInventory
                    });
                    errorCount++;
                    continue;
                }

                //var salePriceDifference = priceAndInventory.SalePrice - productSeller.SalePrice;
                //var salePriceAddition = priceAndInventory.SalePrice + productSeller.SalePrice;
                if (priceAndInventory.SalePrice < 0 || priceAndInventory.ListPrice < 0)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = ApplicationMessage.InvalidPrice.UserMessage(),
                        Item = priceAndInventory
                    });
                    errorCount++;
                    continue;
                }

                var percentDiff = Math.Abs((priceAndInventory.SalePrice - productSeller.SalePrice) / productSeller.SalePrice) * 100;
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
                        StockCount = priceAndInventory.StockCount,
                        StockCode = productSeller.StockCode,
                        ListPrice = priceAndInventory.ListPrice,
                        SalePrice = priceAndInventory.SalePrice,
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
                            Item = priceAndInventory
                        });
                    }
                }
                else
                {
                    productSeller.SetProductPriceAndStock(priceAndInventory.SalePrice, priceAndInventory.ListPrice, priceAndInventory.StockCount);
                    _productSellerRepository.Update(productSeller);
                    await _dbContextHandler.SaveChangesAsync();
                }
            }

            return new ResponseBase<List<UpdatePriceControlResult>>()
            {
                Data = response.Data,
                Success = errorCount > 0 ? false : true,
            };


        }
    }
}