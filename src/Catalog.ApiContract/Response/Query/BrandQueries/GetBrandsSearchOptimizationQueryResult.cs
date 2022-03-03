using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.BrandQueries
{
    public class GetBrandsSearchOptimizationQueryResult
    {
        public DateTime LastDateTime { get; set; }
        public bool Next { get; set; }
        public List<SearchOptimizationBrandList> SearchOptimizationBrandList { get; set; }
    }

    public class SearchOptimizationBrandList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
