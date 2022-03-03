using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.AttributeCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.AttributeCommands
{
    public class UpdateAttributeValueCommandHandler : IRequestHandler<UpdateAttributeValueCommand, ResponseBase<AttributeValueDto>>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAttributeAssembler _attributeAssembler;
        private readonly IGeneralAssembler _generalAssembler;
        public UpdateAttributeValueCommandHandler(IAttributeValueRepository attributeValueRepository,
                                                  IDbContextHandler dbContextHandler,
                                                  IAttributeAssembler attributeAssembler,
                                                  IGeneralAssembler generalAssembler)
        {
            _attributeValueRepository = attributeValueRepository;
            _dbContextHandler = dbContextHandler;
            _attributeAssembler = attributeAssembler;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<AttributeValueDto>> Handle(UpdateAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var existing = await _attributeValueRepository.FindByAsync(c => c.Id == request.Id);
            if (existing != null)
            {
                var seoName = _generalAssembler.GetSeoName(request.Value, Domain.Enums.SeoNameType.AttributeValue);
                existing.SetAttributeValue(request.AttributeId, request.Value, request.Unit, request.Order, seoName);
                _attributeValueRepository.Update(existing);
                await _dbContextHandler.SaveChangesAsync();
                return _attributeAssembler.MapToUpdateAttributeValueCommandResult(existing);
            }

            throw new BusinessRuleException(ApplicationMessage.InvalidId,
            ApplicationMessage.InvalidId.Message(),
            ApplicationMessage.InvalidId.UserMessage());
        }
    }
}
