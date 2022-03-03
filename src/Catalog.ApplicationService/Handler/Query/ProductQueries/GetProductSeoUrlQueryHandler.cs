using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Handler.Services;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    public class GetProductSeoUrlQueryHandler : IRequestHandler<GetProductSeoUrlQuery, ResponseBase<GetProductSeoUrlResponse>>
    {
        private readonly IProductService _productService;

        public GetProductSeoUrlQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ResponseBase<GetProductSeoUrlResponse>> Handle(GetProductSeoUrlQuery request, CancellationToken cancellationToken)
        {
            var result = new GetProductSeoUrlResponse();

            var seoUrl = _productService.GetProductSeoUrl(request.Id);
            result.ProductSeoUrl = seoUrl.Result;

            return new ResponseBase<GetProductSeoUrlResponse>()
            {
                Data = result,
                Success = true
            };
        }
    }
}
