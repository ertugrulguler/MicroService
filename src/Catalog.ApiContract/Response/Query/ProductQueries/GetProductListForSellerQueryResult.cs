using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductListForSellerQueryResult
    {
        public List<SellerProductInfo> Products { get; set; }
        public PageResponse PageResponse { get; set; }
    }
    public class SellerProductInfo
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int PriorityRank { get; set; }
        public string IsVariantable { get; set; }
        public Price ListPrice { get; set; }
        public Price SalePrice { get; set; }
        public int StockCount { get; set; }
        public string StockCode { get; set; }
        public string State { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string GroupCode { get; set; }
    }
}
