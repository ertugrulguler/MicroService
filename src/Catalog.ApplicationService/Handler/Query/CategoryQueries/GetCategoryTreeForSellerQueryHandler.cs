using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoryTreeForSellerQueryHandler : IRequestHandler<GetCategoryTreeForSellerQuery, ResponseBase<CategoryResultForSeller>>
    {
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly ICategoryRepository _categoryRepository;
        public GetCategoryTreeForSellerQueryHandler(ICategoryDomainService categoryDomainService, ICategoryRepository categoryRepository)
        {
            _categoryDomainService = categoryDomainService;
            _categoryRepository = categoryRepository;
        }
        public async Task<ResponseBase<CategoryResultForSeller>> Handle(GetCategoryTreeForSellerQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseBase<CategoryResultForSeller>();
            response.Data = new CategoryResultForSeller();

            response.Data.Main.Categories = await GetCategoryTree();
            response.Success = true;

            return response;
        }

        public async Task<List<CategoryListForSellerDto>> GetCategoryTree()
        {
            var allCategoryList = await _categoryRepository.FilterByAsync(x => x.Type == CategoryTypeEnum.MainCategory);

            var allCategories = MapToGetCategoryTreeQueryResult(allCategoryList);


            var mainCategories = allCategories.Where(x => x.ParentId == null);

            foreach (var mainCategory in mainCategories)
            {
                mainCategory.SubCategories.AddRange(await SubCategorie(mainCategory.Id, allCategories));
            }

            return mainCategories.ToList();
        }

        private async Task<List<CategoryListForSellerDto>> SubCategorie(Guid mainId, List<CategoryListForSellerDto> allCategories)
        {
            var subCategories = allCategories.Where(x => x.ParentId == mainId);

            foreach (var subCategory in subCategories)
            {
                subCategory.SubCategories.AddRange(await SubCategorie(subCategory.Id, allCategories));
            }

            return subCategories.ToList();
        }

        public List<CategoryListForSellerDto> MapToGetCategoryTreeQueryResult(List<Category> categoryList)
        {
            var resultList = new List<CategoryListForSellerDto>();
            foreach (var category in categoryList)
            {
                resultList.Add(new CategoryListForSellerDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Leaf = categoryList.Where(c => c.ParentId == category.Id).Count() == 0,
                    ParentId = category.ParentId,
                });
            }

            return resultList;
        }
    }
}
