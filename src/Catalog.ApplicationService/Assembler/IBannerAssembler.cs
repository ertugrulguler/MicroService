using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using System.Collections.Generic;

namespace Catalog.ApplicationService.Assembler
{
    public interface IBannerAssembler
    {
        ResponseBase<GetBannerListForBO> MapToBannerListQueryResult(List<Banner> banners, List<BannerLocation> bannerLocations);
        ResponseBase<BannerTypeList> MapToBannerTypeListQueryResult(List<BannerType> bannersType);
        ResponseBase<GetBannerActionTypeList> MapToBannerActionTypeListQueryResult(List<BannerActionType> bannersActionType);
        ResponseBase<GetBannerFilterTypeList> MapToBannerFilterTypeListQueryResult(List<BannerFilterType> bannerFilterType);
        ResponseBase<GetBannerLocationListForBO> MapToBannerLocationListQueryResult(List<BannerLocation> bannerLocation);
    }
}
