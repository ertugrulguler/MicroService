using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductIdByCodeQuery : IRequest<ResponseBase<object>>
    {
        public string Code { get; set; }
    }
}
