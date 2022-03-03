using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductsByGroupCodeQuery : IRequest<ResponseBase<GetProductsByGroupCode>>
    {
        public string GroupCode { get; set; }
    }
}
