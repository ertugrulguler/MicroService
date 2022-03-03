using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoryGroupsResponse
    {
        public IEnumerable<CategoryGroup> CategoryGroups { get; set; }
    }

    public class CategoryGroup
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}