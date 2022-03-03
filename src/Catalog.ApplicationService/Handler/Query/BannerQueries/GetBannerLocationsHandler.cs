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
    public class GetBannerLocationsHandler : IRequestHandler<GetBannerLocationsQuery, ResponseBase<GetBannerLocationListForBO>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IBannerAssembler _bannerAssembler;

        public GetBannerLocationsHandler(IBannerRepository bannerRepository, IBannerLocationRepository bannerLocationRepository, IBannerAssembler bannerAssembler)
        {
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
            _bannerAssembler = bannerAssembler;
        }
        public async Task<ResponseBase<GetBannerLocationListForBO>> Handle(GetBannerLocationsQuery request, CancellationToken cancellationToken)
        {

            var getBannerTypeResponseList = new ResponseBase<List<GetBannerLocationListForBO>>
            {
                Data = new List<GetBannerLocationListForBO>()
            };
            var bannerLocations = await _bannerLocationRepository.AllAsync();
            var response = _bannerAssembler.MapToBannerLocationListQueryResult(bannerLocations);

            return new ResponseBase<GetBannerLocationListForBO>
            {
                Data = new GetBannerLocationListForBO
                {
                    GetBannerLocationList = response.Data.GetBannerLocationList
                },
                Success = true
            };

        }
    }
}