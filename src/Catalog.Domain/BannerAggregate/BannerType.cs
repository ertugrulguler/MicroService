using Catalog.Domain.Entities;

namespace Catalog.Domain.BannerAggregate
{
    public class BannerType : Entity
    {
        public int? Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        protected BannerType()
        {

        }

        public BannerType(int type, string description, string name) : this()
        {
            Type = type;
            Description = description;
            Name = name;
        }

        public void SetBannerAction(int type, string description, string name)
        {
            Type = type;
            Description = description;
            Name = name;
        }
    }
}