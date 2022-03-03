using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.BannerAggregate
{
    public class Banner : Entity
    {
        public Guid BannerLocationId { get; protected set; }
        public Guid? ActionId { get; protected set; }
        public Enums.BannerActionType ActionType { get; protected set; }
        public string Name { get; protected set; }
        public string ImageUrl { get; protected set; }
        public int Order { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public string Description { get; set; }
        public int? MMActionId { get; protected set; }
        public string MinIosVersion { get; set; }
        public string MinAndroidVersion { get; set; }
        protected Banner()
        {

        }

        public Banner(Guid bannerLocationId, Enums.BannerActionType actionType, string name, Guid? actionId,
           string imageUrl, int order, DateTime startDate, DateTime endDate, string description, int? mMActionId,
           string minIosVersion, string minAndroidVersion) : this()
        {
            BannerLocationId = bannerLocationId;
            ActionType = actionType;
            Name = name;
            ActionId = actionId;
            ImageUrl = imageUrl;
            Order = order;
            ImageUrl = imageUrl;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            MMActionId = mMActionId;
            MinIosVersion = minIosVersion;
            MinAndroidVersion = minAndroidVersion;
        }
        public void UpdateBanner(string imageUrl, int order, DateTime startdate, DateTime endDate)
        {
            ImageUrl = imageUrl;
            Order = order;
            StartDate = startdate;
            EndDate = endDate;

        }
        public void UpdateBannerAndMMActionId(string imageUrl, int order, int? mMActionId, DateTime startdate, DateTime endDate)
        {
            ImageUrl = imageUrl;
            Order = order;
            MMActionId = mMActionId;
            StartDate = startdate;
            EndDate = endDate;
        }



    }
}
