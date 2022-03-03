using Catalog.ApiContract.Request.Query.CategoryQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.CategoryAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using GetCategoryAndImageBySellerIdResult = Catalog.ApiContract.Response.Query.CategoryQueries.GetCategoryAndImageBySellerIdResult;

namespace Catalog.ApplicationService.Handler.Query.CategoryQueries
{

    public class GetCategoryAndImageBySellerIdHandler : IRequestHandler<GetCategoryAndImageBySellerIdQuery,
        ResponseBase<List<GetCategoryAndImageBySellerIdResult>>>
    {
        private readonly ICategoryDomainService _categoryDomainService;
        private readonly ICategoryAssembler _categoryAssembler;
        public GetCategoryAndImageBySellerIdHandler(ICategoryDomainService categoryDomainService, ICategoryAssembler categoryAssembler)
        {
            _categoryDomainService = categoryDomainService;
            _categoryAssembler = categoryAssembler;
        }

        public async Task<ResponseBase<List<GetCategoryAndImageBySellerIdResult>>> Handle(
            GetCategoryAndImageBySellerIdQuery request, CancellationToken cancellationToken)
        {
            var getCategoryAndImageBySellerId = await _categoryDomainService.GetCategoryAndImageBySellerId(request.SellerId);
            return _categoryAssembler.MapToGetCategoryAndImageBySellerIdResult(getCategoryAndImageBySellerId);
        }
    }
}
