using Catalog.Domain.Entities;

using Framework.Core.Model.Enums;

using System;
using System.Collections.Generic;

namespace Catalog.Domain.BannerAggregate
{
    public class BannerLocation : Entity
    {
        public Enums.BannerType BannerType { get; set; }
        public int Order { get; set; }
        public Enums.BannerLocationType? Location { get; set; }
        public string Title { get; set; } //haftanın ürünleri
        public string Description { get; set; }
        public Guid? ActionId { get; set; }
        private readonly List<Banner> _banners;
        public IReadOnlyCollection<Banner> Banners => _banners;
        public ChannelCode? ProductChannelCode { get; set; }

        protected BannerLocation()
        {
        }

        public BannerLocation(Enums.BannerType bannerType, string title, int order, Enums.BannerLocationType location, string description, ChannelCode? productChannelCode, Guid? actionId) : this()
        {
            BannerType = bannerType;
            Location = location;
            Title = title;
            Order = order;
            Description = description;
            ProductChannelCode = productChannelCode;
            ActionId = actionId;
            _banners = new List<Banner>();
        }

        public void SetBannerLocation(string title, int order, bool isActive)
        {
            Title = title;
            Order = order;
            IsActive = isActive;
        }
    }
}