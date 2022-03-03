using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class CheckCategoryIsExistOrLeafQueryHandler : IRequestHandler<CheckCategoryIsExistOrLeafQuery, ResponseBase<object>>
    {
        private readonly ICategoryDomainService _categoryDomainService;

        public CheckCategoryIsExistOrLeafQueryHandler(ICategoryDomainService categoryDomainService)
        {
            _categoryDomainService = categoryDomainService;
        }

        public async Task<ResponseBase<object>> Handle(CheckCategoryIsExistOrLeafQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryDomainService.CheckCategoryIsExistOrLeaf(request.CategoryId);
            return new ResponseBase<object>() { Data = category, Success = category.Count > 0 ? false : true };
        }
    }
}
