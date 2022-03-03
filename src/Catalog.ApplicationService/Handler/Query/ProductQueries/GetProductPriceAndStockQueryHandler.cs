using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductPriceAndStockQueryHandler : IRequestHandler<GetProductPriceAndStockQuery, ResponseBase<GetProductPriceAndStockQueryResult>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductPriceAndStockQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ResponseBase<GetProductPriceAndStockQueryResult>> Handle(GetProductPriceAndStockQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductSellerInfo(request.SellerId, request.Code);
            if (product == null)
            {
                return new ResponseBase<GetProductPriceAndStockQueryResult>
                {
                    Data = null,
                    Success = false,
                    UserMessage = ApplicationMessage.InvalidCatalogProductId.UserMessage(),
                    Message = ApplicationMessage.InvalidCatalogProductId.UserMessage(),
                    MessageCode = ApplicationMessage.InvalidCatalogProductId
                };
            }

            return new ResponseBase<GetProductPriceAndStockQueryResult>
            {
                Data = new GetProductPriceAndStockQueryResult
                {
                    ListPrice = product.ProductSellers.FirstOrDefault().ListPrice,
                    SalePrice = product.ProductSellers.FirstOrDefault().SalePrice,
                    StockCount = product.ProductSellers.FirstOrDefault().StockCount
                },
                Success = true
            };
        }
    }
}