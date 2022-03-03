using Framework.Core.Model.Enums;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerLocationListForBO
    {
        public List<GetBannerLocationLists> GetBannerLocationList { get; set; }
    }
    public class GetBannerLocationLists
    {
        public Guid BannerLocationId { get; set; }
        public Domain.Enums.BannerType BannerType { get; set; }
        public int Order { get; set; }
        public Domain.Enums.BannerLocationType? Location { get; set; }
        public string Title { get; set; } //haftanın ürünleri
        public string Description { get; set; }
        public Guid? ActionId { get; set; }
        public ChannelCode? ProductChannelCode { get; set; }
    }
}
