using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductsFilterQueryHandler : IRequestHandler<GetProductsFilterQuery,
        ResponseBase<GetProductsFilterQueryResult>>
    {
        private readonly Catalog.ApplicationService.Handler.Services.v2.IProductService _productServiceV2;
        public GetProductsFilterQueryHandler(Catalog.ApplicationService.Handler.Services.v2.IProductService productServiceV2)
        {
            _productServiceV2 = productServiceV2;
        }

        public async Task<ResponseBase<GetProductsFilterQueryResult>> Handle(GetProductsFilterQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _productServiceV2.GetProductListAndFilterV2(request);
            return response;
        }
    }
}