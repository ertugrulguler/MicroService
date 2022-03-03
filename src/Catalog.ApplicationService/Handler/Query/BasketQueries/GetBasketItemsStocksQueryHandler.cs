using Catalog.ApiContract.Request.Query.BasketQueries;
using Catalog.ApiContract.Response.Query.BasketQueries;
using Catalog.ApplicationService.Communicator.Merchant;
using Catalog.ApplicationService.Communicator.Merchant.Model;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BasketQueries
{
    public class GetBasketItemsStocksQueryHandler : IRequestHandler<GetBasketItemsStocksQuery, ResponseBase<List<StockDetail>>>
    {
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IMerhantCommunicator _merchantCommunicator;

        public GetBasketItemsStocksQueryHandler(IProductSellerRepository productSellerRepository, IMerhantCommunicator merchantCommunicator)
        {
            _productSellerRepository = productSellerRepository;
            _merchantCommunicator = merchantCommunicator;
        }
        public async Task<ResponseBase<List<StockDetail>>> Handle(GetBasketItemsStocksQuery request, CancellationToken cancellationToken)
        {
            var stockItems = new List<StockDetail>();
            //var alternateSellerStock = new StockDetail();

            foreach (var requestItem in request.BasketProducts)
            {
                try
                {
                    var productSeller = await _productSellerRepository.FindByAsync(x => x.ProductId == requestItem.ProductId && x.SellerId == requestItem.SellerId);

                    if (productSeller == null)
                        continue;

                    //if (requestItem.Quantity > productSeller.StockCount)
                    //{
                    //    var alternativeSellers = await _productSellerRepository.FilterByAsync(x => x.ProductId == requestItem.ProductId);
                    //    if (alternativeSellers != null && alternativeSellers.Count > 0)
                    //    {
                    //        var cheapestAlternateSeller = alternativeSellers.OrderBy(o => o.SalePrice).FirstOrDefault();
                    //        alternateSellerStock = new StockDetail
                    //        {
                    //            SellerId = cheapestAlternateSeller.SellerId,
                    //            SalePrice = new Price(cheapestAlternateSeller.SalePrice),
                    //            ListPrice = new Price(cheapestAlternateSeller.ListPrice),
                    //            StockCount = cheapestAlternateSeller.StockCount,
                    //            SellerName = await GetSellerName(cheapestAlternateSeller.SellerId)
                    //        };
                    //    }

                    //}

                    stockItems.Add(new StockDetail
                    {
                        //AlternativeSellerStock = alternateSellerStock,
                        ProductId = productSeller.ProductId,
                        SellerId = productSeller.SellerId,
                        ListPrice = new Price(productSeller.ListPrice),
                        SalePrice = new Price(productSeller.SalePrice),
                        StockCount = productSeller.StockCount,
                        SellerName = await GetSellerName(productSeller.SellerId)
                    });
                }
                catch (Exception ex)
                {

                    stockItems.Add(new StockDetail
                    {
                        ProductId = requestItem.ProductId,
                        SellerId = requestItem.SellerId,
                        HasError = true,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return new ResponseBase<List<StockDetail>> { Data = stockItems, Success = true };
        }
        private async Task<string> GetSellerName(Guid sellerId)
        {
            var seller = await _merchantCommunicator.GetSellerById(new GetSellerRequest { SellerId = sellerId });
            try
            {
                return seller.Data.FirmName;
            }
            catch
            {
                return "Seller info not found";
            }

        }
    }


}

