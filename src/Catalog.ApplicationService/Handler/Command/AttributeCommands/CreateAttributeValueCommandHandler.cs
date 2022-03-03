using Catalog.ApiContract.Request.Command.AttributeCommands;
using Catalog.ApiContract.Response.Command.AttributeCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.AttributeCommands
{
    public class CreateAttributeValueCommandHandler : IRequestHandler<CreateAttributeValueCommand, ResponseBase<CreateAttributeValue>>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAttributeAssembler _attributeAssembler;
        private readonly IGeneralAssembler _generalAssembler;

        public CreateAttributeValueCommandHandler(IAttributeRepository attributeRepository,
                                                  IAttributeValueRepository attributeValueRepository,
                                                  IDbContextHandler dbContextHandler,
                                                  IAttributeAssembler attributeAssembler,
                                                  IGeneralAssembler generalAssembler)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
            _dbContextHandler = dbContextHandler;
            _attributeAssembler = attributeAssembler;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<CreateAttributeValue>> Handle(CreateAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var existing = await _attributeRepository.FindByAsync(x => x.Id == request.AttributeId);
            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                    ApplicationMessage.InvalidId.Message(),
                    ApplicationMessage.InvalidId.UserMessage());
            }
            var seoName = _generalAssembler.GetSeoName(request.Value, Domain.Enums.SeoNameType.AttributeValue);
            var value = new AttributeValue(request.AttributeId, request.Value, request.Unit, request.Order, seoName);
            await _attributeValueRepository.SaveAsync(value);
            await _dbContextHandler.SaveChangesAsync();

            return _attributeAssembler.MapToCreateAttributeValueCommandResult(value);
        }
    }
}
