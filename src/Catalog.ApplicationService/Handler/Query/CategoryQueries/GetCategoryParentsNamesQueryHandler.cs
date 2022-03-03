using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApplicationService.Handler.Services;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetCategoryParentsNamesQueryHandler : IRequestHandler<GetCategoryParentsNamesQuery, ResponseBase<Dictionary<Guid, string>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        public GetCategoryParentsNamesQueryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }
        public async Task<ResponseBase<Dictionary<Guid, string>>> Handle(GetCategoryParentsNamesQuery request,
            CancellationToken cancellationToken)
        {

            var categoryList = await _categoryRepository.FilterByAsync(c => request.CategoryIds.Contains(c.Id));
            var result = new ResponseBase<Dictionary<Guid, string>>
            {
                Data = await _categoryService.CategoryWithNameParents(categoryList),
                Success = true
            };
            return result;
        }
    }
}
