using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetCategoryTreeSearchOptimizationQueryResult
    {
        public DateTime LastDateTime { get; set; }
        public bool Next { get; set; }
        public List<SearchOptimizationCategoryTree> SearchOptimizationCategoryTree { get; set; }
    }
    public class SearchOptimizationCategoryTree
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Code { get; set; }
        public List<string> ParentCategories { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public bool Leaf { get; set; }

    }
}
