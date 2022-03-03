using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class ProductFilterWithCount
    {
        public List<Products> ProductList { get; set; }
        public List<Products> AllProductList { get; set; }
        public int TotalCount { get; set; }
    }

    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ProductCode { get; set; }
        public Guid ProductSellerId { get; set; }
        public Guid SellerId { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public string BrandName { get; set; }
        public Guid BrandId { get; set; }
        // public decimal DiscountRate { get; set; }
    }

    public class ProductImages
    {
        public string ImageUrl { get; set; }
    }

}
