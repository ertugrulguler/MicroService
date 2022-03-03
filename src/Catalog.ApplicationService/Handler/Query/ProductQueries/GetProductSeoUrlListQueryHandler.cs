using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.ProductQueries
{
    class GetProductSeoUrlListQueryHandler : IRequestHandler<GetProductSeoUrlListQuery,
        ResponseBase<List<GetProductSeoUrlListResponse>>>
    {
        private readonly IProductService _productService;

        public GetProductSeoUrlListQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseBase<List<GetProductSeoUrlListResponse>>> Handle(GetProductSeoUrlListQuery request,
            CancellationToken cancellationToken)
        {
            var result = new List<GetProductSeoUrlListResponse>();
            foreach (var productId in request.ProductId)
            {
                result.Add(item: new GetProductSeoUrlListResponse
                {
                    ProductId = productId,
                    ProductSeoUrl = (productId == Guid.Empty) ? " " : _productService.GetProductSeoUrl(productId).Result
                });
            }

            return new ResponseBase<List<GetProductSeoUrlListResponse>>
            {
                Data = result,
                Success = true,
                MessageCode = ApplicationMessage.Success
            };
        }
    }
}
