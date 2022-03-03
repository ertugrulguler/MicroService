using Catalog.Domain.Entities;

namespace Catalog.Domain.BannerAggregate
{
    public class BannerFilterType : Entity
    {
        public Enums.BannerFilterType? Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        protected BannerFilterType()
        {

        }

        public BannerFilterType(Enums.BannerFilterType bannerFilterType, string description, string name) : this()
        {
            Type = bannerFilterType;
            Description = description;
            Name = name;
        }

        public void SetBannerAction(Enums.BannerFilterType bannerFilterType, string description, string name)
        {
            Type = bannerFilterType;
            Description = description;
            Name = name;
        }
    }
}