using Catalog.ApiContract.Response.Query.CategoryQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.CategoryQueries
{
    public class GetCategoryGroupsQuery : IRequest<ResponseBase<GetCategoryGroupsResponse>>
    {

    }
}