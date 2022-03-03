using Catalog.ApiContract.Response.Query.ProductQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.ProductQueries
{
    public class GetProductSearchQuery : IRequest<ResponseBase<GetProductSearchNameOrCodeQueryResult>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Size { get; set; }
        public int? Page { get; set; }
    }
}
