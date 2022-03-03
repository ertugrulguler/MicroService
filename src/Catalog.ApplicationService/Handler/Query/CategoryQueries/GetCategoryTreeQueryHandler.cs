using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.Enums;

using Framework.Core.Attribute;
using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoryTreeQueryHandler : IRequestHandler<GetCategoryTreeQuery, ResponseBase<List<CategoryTree>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAssembler _categoryAssembler;
        private readonly ICategoryService _categoryService;
        public GetCategoryTreeQueryHandler(ICategoryRepository categoryRepository, ICategoryAssembler categoryAssembler, ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryAssembler = categoryAssembler;
            _categoryService = categoryService;
        }
        [CacheInfoAttribute(12)]
        public async Task<ResponseBase<List<CategoryTree>>> Handle(GetCategoryTreeQuery request,
            CancellationToken cancellationToken)
        {
            var categoryList = await _categoryRepository.FilterByAsync(c => c.Type == CategoryTypeEnum.MainCategory);

            var parentCategoryDic = new Dictionary<Guid, List<string>>();
            foreach (var item in categoryList)
            {
                var categoryname = _categoryService.CategoryWithParents(item.Id, categoryList);
                parentCategoryDic.Add(item.Id, categoryname.Select(x => x.Name).Reverse().ToList());
            }

            if (categoryList == null)
            {
                throw new BusinessRuleException(ApplicationMessage.EmptyCategoryList,
                    ApplicationMessage.EmptyCategoryList.Message(),
                    ApplicationMessage.EmptyCategoryList.UserMessage());
            }

            return _categoryAssembler.MapToGetCategoryTreeQueryResult(categoryList, parentCategoryDic);
        }
    }
}