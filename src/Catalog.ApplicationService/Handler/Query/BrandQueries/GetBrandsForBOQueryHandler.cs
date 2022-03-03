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
    public class GetBrandsForBOQueryHandler : IRequestHandler<GetBrandsQueryForBO, ResponseBase<List<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IBrandAssembler _brandAssembler;

        public GetBrandsForBOQueryHandler(IBrandRepository brandRepository, IBrandAssembler brandAssembler)
        {
            _brandRepository = brandRepository;
            _brandAssembler = brandAssembler;
        }

        public async Task<ResponseBase<List<BrandDto>>> Handle(GetBrandsQueryForBO request, CancellationToken cancellationToken)
        {
            request.Name ??= "";

            var brand = await _brandRepository.GetBrandsForBO(request.Name, new(request.Page, request.Size));
            if (brand != null)
                return _brandAssembler.MapToGetBrandQueryResultForBO(brand);

            throw new BusinessRuleException(ApplicationMessage.EmptyList,
            ApplicationMessage.EmptyList.Message(),
            ApplicationMessage.EmptyList.UserMessage());
        }
    }
}

