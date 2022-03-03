using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoriesByNameQueryResult
    {
        public Dictionary<string, Guid> CategoryName { get; set; }
    }
}
