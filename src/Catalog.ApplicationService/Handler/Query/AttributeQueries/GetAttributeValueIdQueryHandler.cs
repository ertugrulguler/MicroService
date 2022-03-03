using Catalog.ApiContract.Request.Query.AttributeQueries;
using Catalog.ApiContract.Response.Query.AttributeQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.AttributeQueries
{
    public class GetAttributeValueIdQueryHandler : IRequestHandler<GetAttributeValueIdQuery, ResponseBase<GetAttributeValueId>>
    {
        private readonly IAttributeValueService _attributeValueService;

        public GetAttributeValueIdQueryHandler(IAttributeValueService attributeValueService)
        {
            _attributeValueService = attributeValueService;
        }

        public async Task<ResponseBase<GetAttributeValueId>> Handle(GetAttributeValueIdQuery request, CancellationToken cancellationToken)
        {
            var attValueId = await _attributeValueService.GetAttributeValueId(request.Value);
            if (attValueId == null || attValueId == System.Guid.Empty)
            {
                throw new BusinessRuleException(ApplicationMessage.AttributeValueNotFound,
                    ApplicationMessage.AttributeValueNotFound.Message(),
                    ApplicationMessage.AttributeValueNotFound.UserMessage());
            }
            return new ResponseBase<GetAttributeValueId> { Data = new GetAttributeValueId { Id = attValueId } };
        }
    }
}
