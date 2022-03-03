using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BrandQueries
{
    public class GetBrandIdAndNameQuery
    {
        public Dictionary<string, Guid> BrandName { get; set; }
    }
}
