using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApiContract.Response.Query.CategoryQueries;
using Catalog.Domain.CategoryAggregate;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{
    public class GetActiveCategoryByCategoryIdQueryHandler : IRequestHandler<GetActiveCategoryByCategoryId, ResponseBase<GetActiveCategoryByCategoryIdResult>>

    {
        private readonly ICategoryRepository _categoryRepository;

        public GetActiveCategoryByCategoryIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }

        public async Task<ResponseBase<GetActiveCategoryByCategoryIdResult>> Handle(GetActiveCategoryByCategoryId request,
        CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.FindByAsync(y => y.Id == request.CategoryId && y.IsActive &&
             y.Code != null &&
            y.Type == Domain.Enums.CategoryTypeEnum.MainCategory);
            if (category == null)
                return new ResponseBase<GetActiveCategoryByCategoryIdResult>() { Data = new GetActiveCategoryByCategoryIdResult { IsExist = false } };
            else return new ResponseBase<GetActiveCategoryByCategoryIdResult>() { Data = new GetActiveCategoryByCategoryIdResult { IsExist = true } };
        }
    }
}
