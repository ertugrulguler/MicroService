using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.BrandQueries
{
    public class GetSeoBrandsQuery : IRequest<ResponseBase<GetProductsFilterQueryResult>>
    {
        public string Url { get; set; }
    }
}
