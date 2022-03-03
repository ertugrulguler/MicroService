using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Framework.Core.Attribute;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoriesForMobileQueryHandler : IRequestHandler<GetCategoriesForMobileQuery, ResponseBase<CategoryResultForMobile>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryImageRepository _categoryImageRepository;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryDomainService _categoryDomainService;
        public GetCategoriesForMobileQueryHandler(ICategoryRepository categoryRepository, ICategoryImageRepository categoryImageRepository, ICategoryAssembler categoryAssembler, ICategoryDomainService categoryDomainService)
        {
            _categoryRepository = categoryRepository;
            _categoryImageRepository = categoryImageRepository;
            _categoryAssembler = categoryAssembler;
            _categoryDomainService = categoryDomainService;
        }
        [CacheInfoAttribute(12)]
        public async Task<ResponseBase<CategoryResultForMobile>> Handle(GetCategoriesForMobileQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<CategoryResultForMobile>();
            response.Data = new CategoryResultForMobile();
            var mainCategories = new List<Category>();

            mainCategories = await _categoryRepository.FilterByAsync(c =>
                c.ParentId == null && c.Type == CategoryTypeEnum.MainCategory);

            foreach (var item in mainCategories.OrderBy(c => c.DisplayOrder))
            {

                await item.LoadCategoryImage(_categoryImageRepository);
                var categoryDto = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(item);


                var firstSubCategories = await _categoryRepository.FilterByAsync(c => c.ParentId == item.Id && c.Type == CategoryTypeEnum.MainCategory && c.HasProduct);
                categoryDto.Leaf = firstSubCategories.Count == 0;
                if (!categoryDto.Leaf)
                {
                    foreach (var firstSubItem in firstSubCategories.OrderBy(c => c.DisplayOrder))
                    {
                        await firstSubItem.LoadCategoryImage(_categoryImageRepository);
                        var firstSubCategoryDto = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(firstSubItem);
                        var secondSubCategories = await _categoryRepository.FilterByAsync(c => c.ParentId == firstSubItem.Id && c.Type == CategoryTypeEnum.MainCategory && c.HasProduct);
                        firstSubCategoryDto.Leaf = secondSubCategories.Count == 0;
                        if (!firstSubCategoryDto.Leaf)
                        {
                            foreach (var secondSubItem in secondSubCategories.OrderBy(c => c.DisplayOrder))
                            {
                                await secondSubItem.LoadCategoryImage(_categoryImageRepository);
                                var secondSubCategoryDto = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(secondSubItem);
                                secondSubCategoryDto.Leaf = true;
                                firstSubCategoryDto.SubCategories.Add(secondSubCategoryDto);
                            }

                            if (firstSubItem.HasAll)
                            {

                                firstSubCategoryDto.SubCategories.Add(new CategoryListForMobileDto
                                {
                                    Id = firstSubItem.Id,
                                    ParentId = firstSubItem.ParentId,
                                    Name = "Tümü",
                                    ImageUrl = firstSubItem.CategoryImage == null ? "" : firstSubItem.CategoryImage.Url,
                                    Leaf = true,
                                    Url = firstSubItem.SeoName + "-k-" + firstSubItem.Code
                                });
                            }
                        }

                        categoryDto.SubCategories.Add(firstSubCategoryDto);
                    }
                }

                var suggestedCategories = await _categoryRepository.FilterByAsync(c => c.SuggestedMainId == item.Id);
                foreach (var suggestedCategory in suggestedCategories)
                {
                    await suggestedCategory.LoadCategoryImage(_categoryImageRepository);
                    var mappedItem = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(suggestedCategory);
                    mappedItem.Leaf = true;
                    categoryDto.SuggestedCategories.Add(mappedItem);
                }

                response.Data.Main.Categories.Add(categoryDto);
            }

            var virtualCategories = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.Virtual);

            if (virtualCategories.Count > 0)
            {
                foreach (var item in virtualCategories)
                {
                    await item.LoadCategoryImage(_categoryImageRepository);
                    var virtualSuggestedCategory = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(item);
                    virtualSuggestedCategory.Leaf = true;
                    response.Data.Virtual.Categories.Add(virtualSuggestedCategory);
                }
            }
            else
            {
                response.Data.Virtual = null;
            }

            var chosenForYouCategories = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.ChosenForYou);
            if (chosenForYouCategories.Count > 0)
            {
                var chosenForYou = new CategoryModelForMobileDto("Sizin İçin Seçtiklerimiz");
                foreach (var item in chosenForYouCategories)
                {
                    await item.LoadCategoryImage(_categoryImageRepository);
                    var chosenForYouCategory = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(item);
                    chosenForYouCategory.Leaf = true;
                    chosenForYou.Categories.Add(chosenForYouCategory);
                }
                response.Data.Other.Add(chosenForYou);
            }

            var saleDayMaxCategories = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.SaleDayMax);
            if (saleDayMaxCategories.Count > 0)
            {
                var saleDayMax = new CategoryModelForMobileDto("Maximum İndirim Günleri");
                foreach (var item in saleDayMaxCategories)
                {
                    await item.LoadCategoryImage(_categoryImageRepository);
                    var saleDayMaxCategory = _categoryAssembler.MapToGetCategoriesForMobileQueryResult(item);
                    saleDayMaxCategory.Leaf = true;
                    saleDayMax.Categories.Add(saleDayMaxCategory);
                }
                response.Data.Other.Add(saleDayMax);
            }

            response.Success = true;

            return response;
        }
    }
}
