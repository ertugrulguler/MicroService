using Catalog.Domain.Entities;

namespace Catalog.Domain.BannerAggregate
{
    public class BannerActionType : Entity
    {
        public Enums.BannerActionType? Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        protected BannerActionType()
        {

        }

        public BannerActionType(Enums.BannerActionType bannerActionType, string description, string name) : this()
        {
            Type = bannerActionType;
            Description = description;
            Name = name;
        }

        public void SetBannerAction(Enums.BannerActionType bannerActionType, string description, string name)
        {
            Type = bannerActionType;
            Description = description;
            Name = name;
        }
    }
}