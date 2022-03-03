using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class CategoryIsExistQueryHandler : IRequestHandler<CategoryIsExistQuery, ResponseBase<CategoryIsExistQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryIsExistQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseBase<CategoryIsExistQueryResult>> Handle(CategoryIsExistQuery request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Exist(y => y.Id == request.CategoryId);
            if (!category)
                throw new BusinessRuleException(ApplicationMessage.CategoryNotFound,
                ApplicationMessage.CategoryNotFound.Message(),
                ApplicationMessage.CategoryNotFound.UserMessage());
            else return new ResponseBase<CategoryIsExistQueryResult>() { Data = new CategoryIsExistQueryResult { IsExist = true } };
        }
    }
}
