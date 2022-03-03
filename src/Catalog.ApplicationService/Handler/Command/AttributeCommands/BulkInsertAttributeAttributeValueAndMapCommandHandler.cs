using Catalog.ApiContract.Request.Command.AttributeCommands;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.AttributeCommands
{
    public class BulkInsertAttributeAttributeValueAndMapCommandHandler : IRequestHandler<BulkInsertAttributeAttributeValueAndMapCommand, ResponseBase<object>>
    {
        private readonly IAttributeService _attributeService;

        public BulkInsertAttributeAttributeValueAndMapCommandHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        public async Task<ResponseBase<object>> Handle(BulkInsertAttributeAttributeValueAndMapCommand request, CancellationToken cancellationToken)
        {
            var result = _attributeService.ReadFromExcelWithAttributeAllRelation(request.File);
            if (result)
            {
                var update = await _attributeService.UpdateAttributeValueOrder();
                if (!update)
                    throw new BusinessRuleException(ApplicationMessage.AttributeValueOrderError,
                           ApplicationMessage.AttributeValueOrderError.Message(),
                           ApplicationMessage.AttributeValueOrderError.UserMessage());
            }
            return new ResponseBase<object> { Success = result };
        }
    }
}
