using Catalog.ApiContract.Request.Query.ProductQueries;
using Catalog.ApiContract.Response.Query.ProductQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Guid = System.Guid;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetProductsSearchOptimizationQueryHandler : IRequestHandler<GetCategoryTreeSearchOptimizationQuery, ResponseBase<GetCategoryTreeSearchOptimizationQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;

        public GetProductsSearchOptimizationQueryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }

        public async Task<ResponseBase<GetCategoryTreeSearchOptimizationQueryResult>> Handle(GetCategoryTreeSearchOptimizationQuery request, CancellationToken cancellationToken)
        {
            var categoryTreeList = new List<SearchOptimizationCategoryTree>();
            var parentCategoryDic = new Dictionary<Guid, List<string>>();
            GetCategoryTreeSearchOptimizationQueryResult categorySearchQueryResult;

            var categoryList = (await _categoryRepository.FilterByAsync(c =>
                c.Type == CategoryTypeEnum.MainCategory && c.HasProduct)).OrderBy(c => c.ModifiedDate).ToList();

            if (categoryList.Count < 1)
            {
                categorySearchQueryResult = new GetCategoryTreeSearchOptimizationQueryResult
                {
                    Next = false,
                    SearchOptimizationCategoryTree = null,
                    LastDateTime = DateTime.Now
                };
            }

            else
            {
                foreach (var category in categoryList)
                {
                    var categoryName = _categoryService.CategoryWithParents(category.Id, categoryList);
                    parentCategoryDic.Add(category.Id, categoryName.Select(x => x.Name).Reverse().ToList());

                    categoryTreeList.Add(new SearchOptimizationCategoryTree
                    {
                        Id = category.Id,
                        ParentId = category.ParentId,
                        Code = category.Code,
                        Name = category.Name,
                        DisplayName = category.DisplayName,
                        Description = category.Description,
                        ParentCategories = parentCategoryDic[category.Id],
                    });
                }

                categorySearchQueryResult = new GetCategoryTreeSearchOptimizationQueryResult
                {
                    Next = false,
                    SearchOptimizationCategoryTree = categoryTreeList,
                    LastDateTime = categoryList.LastOrDefault().ModifiedDate.Value
                };
            }


            return new ResponseBase<GetCategoryTreeSearchOptimizationQueryResult>
            {
                Data = categorySearchQueryResult,
                Success = true,
            };
        }
    }
}
