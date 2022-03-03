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
    public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, ResponseBase<CreateAttributeResult>>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work
        private readonly IAttributeAssembler _attributeAssembler;
        private readonly IGeneralAssembler _generalAssembler;
        public CreateAttributeCommandHandler(IAttributeRepository attributeRepository, IGeneralAssembler generalAssembler,
            IDbContextHandler dbContextHandler, IAttributeAssembler attributeAssembler)
        {
            _attributeRepository = attributeRepository;
            _dbContextHandler = dbContextHandler;
            _attributeAssembler = attributeAssembler;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<CreateAttributeResult>> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _attributeRepository.FindByAsync(x => x.Name == request.Name);
            var result = new CreateAttributeResult();
            if (existing != null)
            {
                throw new BusinessRuleException(ApplicationMessage.AttributeAlreadyExist,
                    ApplicationMessage.AttributeAlreadyExist.Message(),
                    ApplicationMessage.AttributeAlreadyExist.UserMessage());
            }
            var seoName = _generalAssembler.GetSeoName(request.Name, Domain.Enums.SeoNameType.Attribute);
            var attribute = new Domain.AttributeAggregate.Attribute(request.Name, request.DisplayName, request.Description, seoName);

            await _attributeRepository.SaveAsync(attribute);

            await _dbContextHandler.SaveChangesAsync();
            var query = await _attributeRepository.FindByAsync(f => f.Name == request.Name && f.DisplayName == request.DisplayName && f.Description == request.Description);
            if (query != null)
            {
                result = new CreateAttributeResult
                {
                    Description = request.Description,
                    DisplayName = request.DisplayName,
                    Name = request.DisplayName,
                    Id = query.Id
                };
            }
            return new ResponseBase<CreateAttributeResult>() { Success = true, Data = result };
        }
    }
}
