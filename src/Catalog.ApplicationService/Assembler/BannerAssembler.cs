using Catalog.ApiContract.Response.Query.BannerQueries;
using Catalog.Domain.BannerAggregate;
using Framework.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.ApplicationService.Assembler
{
    public class BannerAssembler : IBannerAssembler
    {
        public ResponseBase<GetBannerListForBO> MapToBannerListQueryResult(List<Banner> banners, List<BannerLocation> bannerLocations)
        {
            var bannerList = new List<GetBannerListForBOs>();
            foreach (var bannerLocation in banners)
            {
                bannerList.Add(new GetBannerListForBOs
                {
                    BannerId = bannerLocation.Id,
                    Title = bannerLocation?.Name,
                    BannerActionType = Enum.GetName(typeof(Domain.Enums.BannerActionType), bannerLocation.ActionType),
                    StartDate = bannerLocation.StartDate.ToUniversalTime().ToString(),
                    EndDate = bannerLocation.EndDate.ToUniversalTime().ToString(),
                    ImageUrl = bannerLocation.ImageUrl,
                    MMActionId = bannerLocation.MMActionId,
                    Order = bannerLocation.Order,
                    ProductChannelCode = (bannerLocations.FirstOrDefault(xx => xx.Id == bannerLocation.BannerLocationId) != null ?
                    bannerLocations.FirstOrDefault(xx => xx.Id == bannerLocation.BannerLocationId).ProductChannelCode : null)

                });
            }
            var sellerProductQueryResult = new GetBannerListForBO
            {
                Banners = bannerList,
            };
            return new ResponseBase<GetBannerListForBO> { Data = sellerProductQueryResult, Success = true };
        }
        public ResponseBase<BannerTypeList> MapToBannerTypeListQueryResult(List<BannerType> bannersType)
        {
            var bannerTypeList = new List<BannersType>();
            foreach (var type in bannersType)
            {
                bannerTypeList.Add(new BannersType
                {
                    Id = type.Id,
                    Description = type.Description,
                    Name = type.Name,
                    Type = (int)type.Type
                });
            }
            var result = new BannerTypeList
            {
                Banners = bannerTypeList,
            };
            return new ResponseBase<BannerTypeList> { Data = result, Success = true };
        }
        public ResponseBase<GetBannerActionTypeList> MapToBannerActionTypeListQueryResult(List<BannerActionType> bannersType)
        {
            var bannerTypeList = new List<GetBannerActionTypeLists>();
            foreach (var type in bannersType)
            {
                bannerTypeList.Add(new GetBannerActionTypeLists
                {
                    Id = type.Id,
                    Description = type.Description,
                    Name = type.Name,
                    Type = (int)type.Type
                });
            }
            var result = new GetBannerActionTypeList
            {
                BannerActionType = bannerTypeList,
            };
            return new ResponseBase<GetBannerActionTypeList> { Data = result, Success = true };
        }
        public ResponseBase<GetBannerFilterTypeList> MapToBannerFilterTypeListQueryResult(List<BannerFilterType> bannerFilterType)
        {
            var bannerFilterTypeList = new List<GetBannerFilterTypeLists>();
            foreach (var type in bannerFilterType)
            {
                bannerFilterTypeList.Add(new GetBannerFilterTypeLists
                {
                    Id = type.Id,
                    Description = type.Description,
                    Name = type.Name,
                    Type = (int)type.Type
                });
            }
            var result = new GetBannerFilterTypeList
            {
                BannerFilterType = bannerFilterTypeList,
            };
            return new ResponseBase<GetBannerFilterTypeList> { Data = result, Success = true };
        }
        public ResponseBase<GetBannerLocationListForBO> MapToBannerLocationListQueryResult(List<BannerLocation> bannerLocation)
        {
            var bannerFilterTypeList = new List<GetBannerLocationLists>();
            foreach (var type in bannerLocation)
            {
                bannerFilterTypeList.Add(new GetBannerLocationLists
                {
                    BannerLocationId = type.Id,
                    ActionId = type.ActionId,
                    BannerType = type.BannerType,
                    Location = type.Location,
                    Order = type.Order,
                    ProductChannelCode = type.ProductChannelCode,
                    Title = type.Title
                });
            }
            var result = new GetBannerLocationListForBO
            {
                GetBannerLocationList = bannerFilterTypeList,
            };
            return new ResponseBase<GetBannerLocationListForBO> { Data = result, Success = true };
        }
    }
}
