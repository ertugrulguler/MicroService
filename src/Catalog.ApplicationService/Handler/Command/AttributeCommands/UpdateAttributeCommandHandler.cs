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
    public class UpdateAttributeCommandHandler : IRequestHandler<UpdateAttributeCommand, ResponseBase<AttributeDto>>
    {
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeAssembler _attributeAssembler;
        private readonly IGeneralAssembler _generalAssembler;
        public UpdateAttributeCommandHandler(IDbContextHandler dbContextHandler, IGeneralAssembler generalAssembler, IAttributeRepository attributeRepository, IAttributeAssembler attributeAssembler)
        {
            _dbContextHandler = dbContextHandler;
            _attributeRepository = attributeRepository;
            _attributeAssembler = attributeAssembler;
            _generalAssembler = generalAssembler;
        }
        public async Task<ResponseBase<AttributeDto>> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _attributeRepository.FindByAsync(c => c.Id == request.Id);
            if (existing != null)
            {
                var seoName = _generalAssembler.GetSeoName(request.Name, Domain.Enums.SeoNameType.Attribute);
                existing.SetAttribute(request.Name, request.DisplayName, request.Description, seoName);
                _attributeRepository.Update(existing);
                await _dbContextHandler.SaveChangesAsync();
                return _attributeAssembler.MapToCreateAttributeCommandResult(existing);
            }

            throw new BusinessRuleException(ApplicationMessage.AttributeNotFound,
            ApplicationMessage.AttributeNotFound.Message(),
            ApplicationMessage.AttributeNotFound.UserMessage());
        }
    }
}
