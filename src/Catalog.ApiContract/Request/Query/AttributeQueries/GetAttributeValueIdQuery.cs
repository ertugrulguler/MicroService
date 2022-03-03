using Catalog.ApiContract.Response.Query.AttributeQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.AttributeQueries
{
    public class GetAttributeValueIdQuery : IRequest<ResponseBase<GetAttributeValueId>>
    {
        public string Value { get; set; }
    }
}
