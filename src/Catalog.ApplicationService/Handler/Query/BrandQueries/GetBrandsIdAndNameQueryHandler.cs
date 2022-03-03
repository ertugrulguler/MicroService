using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;

using Framework.Core.Model;

using MediatR;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BrandQueries
{
    public class GetBrandsIdAndNameQueryHandler : IRequestHandler<GetBrandsIdAndNameQuery, ResponseBase<List<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandAssembler _brandAssembler;

        public GetBrandsIdAndNameQueryHandler(IBrandRepository brandRepository, IBrandAssembler brandAssembler)
        {
            _brandRepository = brandRepository;
            _brandAssembler = brandAssembler;
        }

        public async Task<ResponseBase<List<BrandDto>>> Handle(GetBrandsIdAndNameQuery request, CancellationToken cancellationToken)
        {

            var allBrands = await _brandRepository.FilterByAsync(a => request.BrandIdList.Contains(a.Id));

            if (allBrands != null)
                return _brandAssembler.MapToGetBrandsIdAndNameQueryResult(allBrands);

            throw new BusinessRuleException(ApplicationMessage.EmptyList,
            ApplicationMessage.EmptyList.Message(),
            ApplicationMessage.EmptyList.UserMessage());
        }
    }
}
