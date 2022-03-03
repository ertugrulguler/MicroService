using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoriesByNameQuery : IRequest<ResponseBase<GetCategoriesByNameQueryResult>>
    {
        public string Name { get; set; }
    }
}