using Catalog.Domain.BannerAggregate;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerById
    {
        public Guid BannerLocationId { get; set; }
        public Guid? ActionId { get; set; }
        public Domain.Enums.BannerActionType ActionType { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public int? MMActionId { get; set; }
        public string MinIosVersion { get; set; }
        public string MinAndroidVersion { get; set; }
        public List<BannerLocation> BannerLocation { get; set; }
    }
}
