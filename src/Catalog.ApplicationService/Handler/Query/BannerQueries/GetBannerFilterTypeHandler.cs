using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannerFilterTypeHandler : IRequestHandler<GetBannerFilterTypeQuery, ResponseBase<GetBannerFilterTypeList>>
    {
        private readonly IBannerFilterTypeRepository _bannerFilterTypeRepository;
        private readonly IBannerAssembler _bannerAssembler;

        public GetBannerFilterTypeHandler(IBannerFilterTypeRepository bannerFilterTypeRepository, IBannerAssembler bannerAssembler)
        {
            _bannerFilterTypeRepository = bannerFilterTypeRepository;
            _bannerAssembler = bannerAssembler;
        }
        public async Task<ResponseBase<GetBannerFilterTypeList>> Handle(GetBannerFilterTypeQuery request, CancellationToken cancellationToken)
        {
            var getBannerTypeResponseList = new ResponseBase<List<GetBannerFilterTypeList>>
            {
                Data = new List<GetBannerFilterTypeList>()
            };
            var bannerFilterList = await _bannerFilterTypeRepository.AllAsync();
            var response = _bannerAssembler.MapToBannerFilterTypeListQueryResult(bannerFilterList);

            return new ResponseBase<GetBannerFilterTypeList>
            {
                Data = new GetBannerFilterTypeList
                {
                    BannerFilterType = response.Data.BannerFilterType
                },
                Success = true
            };
        }
    }
}