using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.ApiContract.Response.Command.ProductCommands;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class UpdateProductStockCountCommandHandler : IRequestHandler<UpdateProductStockCountCommand, ResponseBase<List<UpdatePriceControlResult>>>
    {
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDbContextHandler _dbContextHandler;


        public UpdateProductStockCountCommandHandler(IProductSellerRepository productSellerRepository, IProductRepository productRepository, IDbContextHandler dbContextHandler)
        {
            _productSellerRepository = productSellerRepository;
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<List<UpdatePriceControlResult>>> Handle(UpdateProductStockCountCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<List<UpdatePriceControlResult>>();
            response.Data = new List<UpdatePriceControlResult>();
            int errorCount = 0;

            foreach (var inventory in request.Items)
            {
                if (inventory.StockCount < 0)
                {
                    response.Data.Add(new UpdatePriceControlResult()
                    {
                        Error = inventory.Code + ApplicationMessage.StockCountNotZero.UserMessage(),
                        Item = inventory
                    });
                    errorCount++;
                    continue;
                }
                else
                {
                    var product = await _productRepository.FindByAsync(p => p.Code == inventory.Code);
                    if (product == null)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidCatalogProductId.UserMessage(),
                            Item = inventory
                        });
                        errorCount++;
                        continue;
                    }

                    var productSeller = await _productSellerRepository.FindByAsync(ps => ps.ProductId == product.Id && ps.SellerId == request.SellerId);
                    if (productSeller == null)
                    {
                        response.Data.Add(new UpdatePriceControlResult()
                        {
                            Error = ApplicationMessage.InvalidProductId.UserMessage(),
                            Item = inventory
                        });
                        errorCount++;
                        continue;
                    }

                    productSeller.SetProductStock(inventory.StockCount);
                    _productSellerRepository.Update(productSeller);
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