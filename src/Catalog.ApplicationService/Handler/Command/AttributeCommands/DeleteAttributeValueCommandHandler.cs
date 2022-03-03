using Catalog.ApiContract.Contract;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.AttributeAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;


namespace Catalog.ApplicationService.Handler.Command.AttributeCommands
{
    public class DeleteAttributeValueCommandHandler : IRequestHandler<DeleteAttributeValueCommand, ResponseBase<AttributeValueDto>>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAttributeAssembler _attributeAssembler;
        public DeleteAttributeValueCommandHandler(IAttributeValueRepository attributeValueRepository,
                                                  IDbContextHandler dbContextHandler,
                                                  IAttributeAssembler attributeAssembler)
        {
            _attributeValueRepository = attributeValueRepository;
            _dbContextHandler = dbContextHandler;
            _attributeAssembler = attributeAssembler;
        }

        public async Task<ResponseBase<AttributeValueDto>> Handle(DeleteAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var existing = await _attributeValueRepository.FindByAsync(x => x.Id == request.Id);

            if (existing == null)
            {
                throw new BusinessRuleException(ApplicationMessage.InvalidId,
                    ApplicationMessage.InvalidId.Message(),
                    ApplicationMessage.InvalidId.UserMessage());
            }

            _attributeValueRepository.Delete(existing);
            await _dbContextHandler.SaveChangesAsync();
            return _attributeAssembler.MapToDeleteAttributeCommandResult(existing);
        }
    }
}
