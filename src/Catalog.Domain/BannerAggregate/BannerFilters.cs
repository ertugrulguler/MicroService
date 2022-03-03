using Catalog.Domain.Entities;
using System;
namespace Catalog.Domain.BannerAggregate
{
    public class BannerFilters : Entity
    {
        public Guid BannerId { get; protected set; }
        public Guid ActionId { get; protected set; }
        public Enums.BannerFilterType BannerFilterType { get; protected set; }
        protected BannerFilters()
        {

        }

        public BannerFilters(Guid bannerId, Enums.BannerFilterType bannerFilterType, Guid actionId) : this()
        {
            BannerId = bannerId;
            BannerFilterType = bannerFilterType;
            ActionId = actionId;
        }

        public void SetBannerFilters(Enums.BannerFilterType bannerFilterType, Guid actionId)
        {
            BannerFilterType = bannerFilterType;
            ActionId = actionId;
        }
    }
}