using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ResponseBase<GetCategoriesResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryImageRepository _categoryImageRepository;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryDomainService _categoryDomainService;
        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, ICategoryImageRepository categoryImageRepository, ICategoryAssembler categoryAssembler, ICategoryDomainService categoryDomainService)
        {
            _categoryRepository = categoryRepository;
            _categoryImageRepository = categoryImageRepository;
            _categoryAssembler = categoryAssembler;
            _categoryDomainService = categoryDomainService;
        }
        //[CacheInfoAttribute(12)]
        public async Task<ResponseBase<GetCategoriesResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<GetCategoriesResult>();
            //  var mainCategories = new List<Category>();

            var mainCategories = await _categoryRepository.FindByAsync(c => c.Id == request.CategoryId);

            await mainCategories.LoadCategoryImage(_categoryImageRepository);
            var categoryDto = _categoryAssembler.MapToGetCategoriesQueryResult(mainCategories);

            var firstSubCategories = await _categoryRepository.FilterByAsync(c => c.ParentId == mainCategories.Id && c.Type == CategoryTypeEnum.MainCategory);
            categoryDto.Leaf = firstSubCategories.Count == 0;
            if (!categoryDto.Leaf)
            {
                foreach (var firstSubItem in firstSubCategories)
                {
                    await firstSubItem.LoadCategoryImage(_categoryImageRepository);
                    var firstSubCategoryDto = _categoryAssembler.MapToGetCategoriesQueryResult(firstSubItem);
                    var secondSubCategories = await _categoryRepository.FilterByAsync(c => c.ParentId == firstSubItem.Id && c.Type == CategoryTypeEnum.MainCategory);
                    firstSubCategoryDto.Leaf = secondSubCategories.Count == 0;

                    if (!firstSubCategoryDto.Leaf)
                    {
                        foreach (var secondSubItem in secondSubCategories)
                        {
                            await secondSubItem.LoadCategoryImage(_categoryImageRepository);
                            var secondSubCategoryDto = _categoryAssembler.MapToGetCategoriesQueryResult(secondSubItem);
                            secondSubCategoryDto.Leaf = true;
                            firstSubCategoryDto.SubCategories.Add(secondSubCategoryDto);
                        }
                    }

                    categoryDto.SubCategories.Add(firstSubCategoryDto);
                }
            }

            var suggestedCategories = await _categoryRepository.FilterByAsync(c => c.SuggestedMainId == mainCategories.Id);
            if (suggestedCategories.Count > 0)
            {
                foreach (var suggestedCategory in suggestedCategories)
                {
                    await suggestedCategory.LoadCategoryImage(_categoryImageRepository);
                    var mappedItem = _categoryAssembler.MapToGetCategoriesQueryResult(suggestedCategory);
                    mappedItem.Leaf = true;
                    categoryDto.SuggestedCategories.Add(mappedItem);
                }
            }

            response.Data = new GetCategoriesResult
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                ImageUrl = categoryDto.ImageUrl,
                Leaf = categoryDto.Leaf,
                ParentId = categoryDto.ParentId,
                SuggestedCategories = categoryDto.SuggestedCategories,
                SubCategories = categoryDto.SubCategories
            };

            response.Success = true;
            return response;
        }
    }
}
