using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.Domain.ProductAggregate;

using Framework.Core.Model;

using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductCountDetailsQueryHandler : IRequestHandler<GetSellerProductCountDetailsQuery, ResponseBase<GetSellerProductCountsDetail>>
    {

        private readonly IProductRepository _productRepository;

        public GetProductCountDetailsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;

        }
        public async Task<ResponseBase<GetSellerProductCountsDetail>> Handle(GetSellerProductCountDetailsQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductListForSellerForCountSP(request.SellerId);
            var response = new GetSellerProductCountsDetail()
            {
                OnSale = new ProductCountAndStock()
                {
                    ProductCount = product.Select(item => item.OnSaleProductCount).FirstOrDefault(),
                    StockCount = product.Select(item => item.OnSaleStockCount).FirstOrDefault(),
                },
                OnSold = new ProductCountAndStock()
                {
                    ProductCount = product.Select(item => item.OnSoldProductCount).FirstOrDefault(),
                    StockCount = 0
                }
            };

            return new ResponseBase<GetSellerProductCountsDetail>() { Data = response, Success = true };
        }

    }

}
