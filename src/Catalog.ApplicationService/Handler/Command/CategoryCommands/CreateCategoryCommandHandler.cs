using Catalog.ApiContract.Request.Command.CategoryCommands;
using Catalog.ApiContract.Response.Command.CategoryCommands;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;

using Framework.Core.Model;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Command.CategoryCommands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseBase<CreateCategory>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbContextHandler _dbContextHandler; //unit of work
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly IGeneralAssembler _generalAssembler;
        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IDbContextHandler dbContextHandler, IGeneralAssembler generalAssembler,
            ICategoryAssembler categoryAssembler, ICategoryDomainService categoryDomainService)
        {
            _categoryRepository = categoryRepository;
            _dbContextHandler = dbContextHandler;
            _categoryAssembler = categoryAssembler;
            _categoryDomainService = categoryDomainService;
            _generalAssembler = generalAssembler;
        }

        public async Task<ResponseBase<CreateCategory>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _categoryRepository.FindByAsync(x =>
                string.Equals(x.Name, request.Name) ||
                string.Equals(x.DisplayName, request.DisplayName));

            if (existing != null)
            {
                throw new BusinessRuleException(ApplicationMessage.CategoryAlreadyExist,
                    ApplicationMessage.CategoryAlreadyExist.Message(),
                    ApplicationMessage.CategoryAlreadyExist.UserMessage());
            }

            var mainCategoryId = Guid.Empty;
            if (request.Suggested && await _categoryDomainService.CheckSuitableForSuggestedCategory(request.ParentId))
            {
                var mainCategory = await _categoryDomainService.GetMainCategory(request.ParentId.Value);
                mainCategoryId = mainCategory.Id;
            }
            var seoName = _generalAssembler.GetSeoName(request.Name, Domain.Enums.SeoNameType.Category);
            var category = new Category(request.ParentId, request.Name, request.DisplayName,
                request.Code, request.DisplayOrder, request.Description, CategoryTypeEnum.MainCategory, null, request.HasAll, false, null, false, seoName);

            category.SetImage(request.CategoryImage.Name, request.CategoryImage.Url, request.CategoryImage.Description);


            await _categoryRepository.SaveAsync(category);

            await _dbContextHandler.SaveChangesAsync();

            return _categoryAssembler.MapToCrateCategoryCommandResult(category);
        }
    }
}