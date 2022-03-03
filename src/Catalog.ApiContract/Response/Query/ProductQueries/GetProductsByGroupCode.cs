using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductsByGroupCode
    {
        public List<ProductByGroupCode> ProductsByGroupCode { get; set; }
    }

    public class ProductByGroupCode
    {
        public Guid ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string GroupCode { get; set; }
        public List<SellerProductAttribute> Attributes { get; set; }
    }
}
