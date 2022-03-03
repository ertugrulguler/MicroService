using Catalog.ApiContract.Request.Command.BasketCommands;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.BasketCommands
{
    public class RemoveStockCommandHandler : IRequestHandler<RemoveStockCommand, ResponseBase<object>>
    {
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IDbContextHandler _dbContextHandler;

        public RemoveStockCommandHandler(IProductSellerRepository productSellerRepository, IDbContextHandler dbContextHandler)
        {
            _productSellerRepository = productSellerRepository;
            _dbContextHandler = dbContextHandler;
        }

        public async Task<ResponseBase<object>> Handle(RemoveStockCommand request, CancellationToken cancellationToken)
        {
            foreach (var requestItem in request.BasketProducts)
            {
                var productSeller = await _productSellerRepository.FindByAsync(x => x.ProductId == requestItem.ProductId && x.SellerId == requestItem.SellerId);

                if (productSeller == null)
                {
                    throw new BusinessRuleException(ApplicationMessage.EmptySellerProducts,
                         ApplicationMessage.EmptySellerProducts.Message(),
                         ApplicationMessage.EmptySellerProducts.UserMessage());
                }

                if (productSeller.StockCount - requestItem.Quantity >= 0)
                {
                    productSeller.SetStockCount(productSeller.StockCount - requestItem.Quantity);
                    _productSellerRepository.Update(productSeller);
                }
            }
            //TODO: Dikkat
            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<object> { Success = true };
        }
    }
}
