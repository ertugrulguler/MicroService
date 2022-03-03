using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CreateCategoryAttributeCommandHandler : IRequestHandler<CreateCategoryAttributeCommand, ResponseBase<CategoryAttributeDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work
        private readonly ICategoryAssembler _categoryAssembler;

        public CreateCategoryAttributeCommandHandler(ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler,
            ICategoryAssembler categoryAssembler)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryAssembler = categoryAssembler;
        }

        public async Task<ResponseBase<CategoryAttributeDto>> Handle(CreateCategoryAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryRepository.FindByAsync(c => c.Id == request.CategoryId);

                if (category == null)
                {
                    throw new BusinessRuleException(ApplicationMessage.CategoryAlreadyExist,
                    ApplicationMessage.AttributeAlreadyExist.Message(),
                    ApplicationMessage.AttributeAlreadyExist.UserMessage());
                }

                category.VerifyOrAddCategoryAttribute(request.AttributeId, request.IsRequired);

                await _dbContextHandler.SaveChangesAsync();

                return new ResponseBase<CategoryAttributeDto>();

                //return _categoryAssembler.MapToCreateCategoryAttributeCommandResult(category)
            }
            catch (Exception ex)
            {
                return new ResponseBase<CategoryAttributeDto>() { Message = ex.Message };
            }
        }
    }
}
