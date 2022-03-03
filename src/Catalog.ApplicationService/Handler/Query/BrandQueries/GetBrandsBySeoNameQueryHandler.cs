using Catalog.ApiContract.Request.Query.BrandQueries;
using Catalog.ApiContract.Response.Query.BrandQueries;
using Catalog.Domain;
using Catalog.Domain.BrandAggregate;

using Framework.Core.Model;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BrandQueries
{
    public class GetBrandsBySeoNameQueryHandler : IRequestHandler<GetBrandsBySeoNameQuery, ResponseBase<GetBrandIdAndSeoNameQuery>>
    {
        private readonly IBrandDomainService _brandDomainService;
        public GetBrandsBySeoNameQueryHandler(IBrandDomainService brandDomainService)
        {
            _brandDomainService = brandDomainService;
        }

        public async Task<ResponseBase<GetBrandIdAndSeoNameQuery>> Handle(GetBrandsBySeoNameQuery request,
            CancellationToken cancellationToken)
        {
            var brandList = await _brandDomainService.GetBrandName(request.BrandNameList, true);
            if (brandList == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                ApplicationMessage.EmptyList.Message(),
                ApplicationMessage.EmptyList.UserMessage());
            return new ResponseBase<GetBrandIdAndSeoNameQuery>() { Data = new GetBrandIdAndSeoNameQuery { BrandName = brandList }, Success = true };
        }
    }
}
