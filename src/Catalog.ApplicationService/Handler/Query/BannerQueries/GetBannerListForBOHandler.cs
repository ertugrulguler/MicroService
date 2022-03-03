using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.ApplicationService.Assembler;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannerListForBOHandler : IRequestHandler<GetBannerForBOQuery, ResponseBase<GetBannerListForBO>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;
        private readonly IBannerAssembler _bannerAssembler;

        public GetBannerListForBOHandler(IBannerRepository bannerRepository, IBannerLocationRepository bannerLocationRepository, IBannerAssembler bannerAssembler)
        {
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
            _bannerAssembler = bannerAssembler;
        }

        public async Task<ResponseBase<GetBannerListForBO>> Handle(GetBannerForBOQuery request, CancellationToken cancellationToken)
        {
            var getBannerResponseList = new ResponseBase<List<GetBannerListForBO>>
            {
                Data = new List<GetBannerListForBO>()
            };
            var bannerLocationList = await _bannerLocationRepository.GetBannerDetailList();

            var bannerList = await _bannerRepository.FilterByPagingAsync(x => bannerLocationList
                                                                            .Select(y => y.Id)
                                                                            .Contains(x.BannerLocationId), new PagerInput(request.Page, request.Size));

            var response = _bannerAssembler.MapToBannerListQueryResult(bannerList, bannerLocationList);

            return new ResponseBase<GetBannerListForBO>
            {
                Data = new GetBannerListForBO
                {
                    Banners = response.Data.Banners,
                    PageResponse = new PageResponse(bannerList.TotalCount, new(request.Page, request.Size))

                },
                Success = true
            };
        }

    }
}