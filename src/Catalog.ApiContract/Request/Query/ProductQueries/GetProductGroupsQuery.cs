using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductGroupsQuery : IRequest<ResponseBase<List<string>>>
    {
    }
}
