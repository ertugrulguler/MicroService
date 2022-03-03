using Catalog.ApiContract.Request.Query.BannerQueries;
using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query.BannerQueries
{
    public class GetBannerListByIdHandler : IRequestHandler<GetBannerByIdQuery, ResponseBase<GetBannerById>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBannerLocationRepository _bannerLocationRepository;

        public GetBannerListByIdHandler(IBannerRepository bannerRepository, IBannerLocationRepository bannerLocationRepository)
        {
            _bannerRepository = bannerRepository;
            _bannerLocationRepository = bannerLocationRepository;
        }
        public async Task<ResponseBase<GetBannerById>> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
        {
            var getBannerResponseList = new ResponseBase<List<BannerLocation>>
            {
                Data = new List<BannerLocation>()
            };

            var bannerList = await _bannerRepository.FindByAsync(x => x.Id == request.Id);

            var getBannerResponse = new ResponseBase<Banner>
            {
                Data = bannerList
            };

            var bannerLocationDetail = await _bannerLocationRepository.FilterByAsync(x => x.Id == bannerList.BannerLocationId);
            getBannerResponseList.Data.AddRange(bannerLocationDetail);
            getBannerResponseList.Success = true;

            return new ResponseBase<GetBannerById>()
            {
                Data = new GetBannerById
                {
                    BannerLocationId = bannerList.BannerLocationId,
                    ActionId = bannerList.ActionId,
                    ActionType = bannerList.ActionType,
                    Name = bannerList.Name,
                    ImageUrl = bannerList.ImageUrl,
                    Order = bannerList.Order,
                    StartDate = bannerList.StartDate.ToUniversalTime().ToString(),
                    EndDate = bannerList.EndDate.ToUniversalTime().ToString(),
                    Description = bannerList.Description,
                    MMActionId = bannerList.MMActionId,
                    MinIosVersion = bannerList.MinIosVersion,
                    MinAndroidVersion = bannerList.MinAndroidVersion,
                    BannerLocation = bannerLocationDetail
                },
                Success = true
            };


        }
    }
}