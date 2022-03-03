using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerFilterTypeList
    {
        public List<GetBannerFilterTypeLists> BannerFilterType { get; set; }
    }
    public class GetBannerFilterTypeLists
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }


}
