using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CreateVirtualCategoryCommandHandler : IRequestHandler<CreateVirtualCategoryCommand, ResponseBase<CreateVirtualCategoryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work
        private readonly IGeneralAssembler _generalAssembler;
        public CreateVirtualCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler, IGeneralAssembler generalAssembler)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<CreateVirtualCategoryResult>> Handle(CreateVirtualCategoryCommand request, CancellationToken cancellationToken)
        {
            var existing = await _categoryRepository.FindByAsync(x => string.Equals(x.Name, request.Name) || string.Equals(x.DisplayName, request.DisplayName));

            if (existing != null)
            {
                throw new BusinessRuleException(ApplicationMessage.CategoryAlreadyExist,
                    ApplicationMessage.CategoryAlreadyExist.Message(),
                    ApplicationMessage.CategoryAlreadyExist.UserMessage());
            }
            var seoName = _generalAssembler.GetSeoName(request.Name, SeoNameType.Category);
            var virtualCategory = new Category(null, request.Name, request.DisplayName, "VRTCTGRY",
                1, request.Description, CategoryTypeEnum.VipSellerVirtual, null, false, false, null, true, seoName);


            await _categoryRepository.SaveAsync(virtualCategory);

            await _dbContextHandler.SaveChangesAsync();

            return new ResponseBase<CreateVirtualCategoryResult>
            {
                Data = new CreateVirtualCategoryResult
                {
                    Id = virtualCategory.Id,
                    Name = virtualCategory.DisplayName
                },
                Success = true
            };
        }
    }
}