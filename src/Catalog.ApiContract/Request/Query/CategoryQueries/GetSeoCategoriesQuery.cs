using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetSeoCategoriesQuery : IRequest<ResponseBase<GetProductsFilterQueryResult>>
    {
        public string Url { get; set; }
    }
}
