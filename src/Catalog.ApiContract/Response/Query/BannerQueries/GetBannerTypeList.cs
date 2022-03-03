using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class BannerTypeList
    {
        public List<BannersType> Banners { get; set; }
    }
    public class BannersType
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }


}
