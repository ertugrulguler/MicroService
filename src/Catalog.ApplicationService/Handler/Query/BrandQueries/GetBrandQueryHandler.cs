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
    public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, ResponseBase<List<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandAssembler _brandAssembler;

        public GetBrandQueryHandler(IBrandRepository brandRepository, IBrandAssembler brandAssembler)
        {
            _brandRepository = brandRepository;
            _brandAssembler = brandAssembler;
        }

        public async Task<ResponseBase<List<BrandDto>>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            request.Name ??= "";

            var brand = await _brandRepository.FilterByPagingAsync(b => b.Name.StartsWith(request.Name), new(request.Page, request.Size));

            if (brand != null)
                return _brandAssembler.MapToGetBrandQueryResult(brand);

            throw new BusinessRuleException(ApplicationMessage.EmptyList,
            ApplicationMessage.EmptyList.Message(),
            ApplicationMessage.EmptyList.UserMessage());
        }
    }
}
