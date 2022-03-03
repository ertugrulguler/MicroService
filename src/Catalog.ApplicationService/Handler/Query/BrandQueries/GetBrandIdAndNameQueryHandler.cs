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
    public class GetBrandIdAndNameQueryHandler : IRequestHandler<GetBrandsByNameQuery, ResponseBase<GetBrandIdAndNameQuery>>
    {
        private readonly IBrandDomainService _brandDomainService;
        public GetBrandIdAndNameQueryHandler(IBrandDomainService brandDomainService)
        {
            _brandDomainService = brandDomainService;
        }

        public async Task<ResponseBase<GetBrandIdAndNameQuery>> Handle(GetBrandsByNameQuery request,
            CancellationToken cancellationToken)
        {
            var brandList = await _brandDomainService.GetBrandName(request.BrandNameList, false);
            if (brandList == null)
                throw new BusinessRuleException(ApplicationMessage.EmptyList,
                ApplicationMessage.EmptyList.Message(),
                ApplicationMessage.EmptyList.UserMessage());
            return new ResponseBase<GetBrandIdAndNameQuery>() { Data = new GetBrandIdAndNameQuery { BrandName = brandList }, Success = true };
        }
    }
}
