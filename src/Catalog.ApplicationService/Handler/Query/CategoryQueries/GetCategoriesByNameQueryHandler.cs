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
    public class GetCategoriesByNameQueryHandler : IRequestHandler<GetCategoriesByNameQuery, ResponseBase<GetCategoriesByNameQueryResult>>
    {
        private readonly ICategoryDomainService _categoryDomainService;
        public GetCategoriesByNameQueryHandler(ICategoryDomainService categoryDomainService)
        {
            _categoryDomainService = categoryDomainService;
        }
        public async Task<ResponseBase<GetCategoriesByNameQueryResult>> Handle(GetCategoriesByNameQuery request, CancellationToken cancellationToken)
        {
            var categoryList = await _categoryDomainService.GetCategoryName(request.Name);

            if (categoryList == null)
            {
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                    ApplicationMessage.EmptyList.Message(),
                    ApplicationMessage.EmptyList.UserMessage());
            }

            return new ResponseBase<GetCategoriesByNameQueryResult>()
            {
                Data = new GetCategoriesByNameQueryResult { CategoryName = categoryList },
                Success = true
            };
        }
    }
}
