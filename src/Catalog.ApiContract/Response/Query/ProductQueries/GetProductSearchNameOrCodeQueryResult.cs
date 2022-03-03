using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductSearchNameOrCodeQueryResult
    {
        public List<ProductSearchList> Products { get; set; }
    }
    public class ProductSearchList
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int VatRate { get; set; }
        public string ImageUrl { get; set; }
        public List<ProductSearchAttributes> Attributes { get; set; }
    }

    public class ProductSearchAttributes
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

}
