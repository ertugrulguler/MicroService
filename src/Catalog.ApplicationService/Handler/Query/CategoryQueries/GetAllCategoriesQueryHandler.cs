using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
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
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ResponseBase<List<CategoryIdAndName>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAssembler _categoryAssembler;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, ICategoryAssembler categoryAssembler)
        {
            _categoryRepository = categoryRepository;
            _categoryAssembler = categoryAssembler;
        }
        public async Task<ResponseBase<List<CategoryIdAndName>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _categoryRepository.FilterByAsync(a => request.CategoryIdList.Contains(a.Id) && a.Type == CategoryTypeEnum.MainCategory);

            if (allCategories.Any())
            {
                var leafCategoryList = new Dictionary<Guid, string>();

                if (request.CategoryIdList.Count() == 1)
                {
                    foreach (var item in allCategories)
                    {
                        if (!string.IsNullOrEmpty(item.LeafPath))
                        {
                            var categoryName = "->" + item.Name;
                            var categoryId = new Guid();
                            var splitCategory = item.LeafPath.Split(',');

                            foreach (var leafItem in splitCategory)
                            {
                                categoryId = new Guid(leafItem);
                                var categoryItem = await _categoryRepository.GetByIdAsync(categoryId);
                                categoryName = "->" + categoryItem.Name + categoryName;
                            }
                            leafCategoryList.Add(item.Id, categoryName.Substring(2, categoryName.Length - 2));
                        }
                    }
                }
                return _categoryAssembler.MapToGetAllCategoriesQueryResult(allCategories, leafCategoryList);
            }

            throw new BusinessRuleException(ApplicationMessage.EmptyList,
                ApplicationMessage.EmptyList.Message(),
                ApplicationMessage.EmptyList.UserMessage());
        }
    }
}
