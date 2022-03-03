using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductsSearchOptimizationQueryResult
    {
        public DateTime LastDateTime { get; set; }
        public bool Next { get; set; }
        public List<SearchOptimizationProductList> SearchOptimizationProductList { get; set; }
    }

    public class SearchOptimizationProductList
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string GroupCode { get; set; }
        public List<SearchOptimizationProductCategories> Categories { get; set; }
        public List<SearchOptimizationProductAttributes> Attributes { get; set; }

    }

    public class SearchOptimizationProductCategories
    {
        public string CategoryName { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class SearchOptimizationProductAttributes
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

}
