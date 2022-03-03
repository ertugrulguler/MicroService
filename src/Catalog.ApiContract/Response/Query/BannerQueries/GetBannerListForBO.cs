using Framework.Core.Model.Enums;
using System;
using System.Collections.Generic;


namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerListForBO
    {
        public List<GetBannerListForBOs> Banners { get; set; }
        public PageResponse PageResponse { get; set; }
    }
    public class GetBannerListForBOs
    {
        public Guid BannerId { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BannerActionType { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public int? MMActionId { get; set; }
        public ChannelCode? ProductChannelCode { get; set; }


    }

}

