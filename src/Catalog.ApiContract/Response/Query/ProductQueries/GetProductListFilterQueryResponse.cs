using System;
using System.Collections.Generic;
using Framework.Core.Model;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductListFilterQueryResponse
    {
        public List<FilterProduct> Products { get; set; }
        public string Breadcrumb { get; set; }
    }
    public class FilterProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; } //Ürünleri farklılaştıran EAN13 Kodu
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string ListPrice { get; set; }
        public string SalePrice { get; set; }
        public string Url { get; set; }
        public IEnumerable<SellerProductImage> ImageList { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public bool IsVariantable { get; set; }
    }

    public class Image
    {
        public string Url { get; set; }
    }
}