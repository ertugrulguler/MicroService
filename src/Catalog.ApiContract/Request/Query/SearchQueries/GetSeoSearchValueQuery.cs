using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Response.Query.SearchQueries
{
    public class GetSeoSearchValueQuery : IRequest<ResponseBase<GetProductsFilterQueryResult>>
    {
        public string Url { get; set; }
    }
}
