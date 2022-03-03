using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BannerQueries
{
    public class GetBannerActionTypeList
    {
        public List<GetBannerActionTypeLists> BannerActionType { get; set; }
    }
    public class GetBannerActionTypeLists
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }


}
