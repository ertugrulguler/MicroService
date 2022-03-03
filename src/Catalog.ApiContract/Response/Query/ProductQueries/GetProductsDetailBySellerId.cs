using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductsDetailBySellerId : ResponseBase
    {
        public List<SellerProducts> Products { get; set; }
    }

    public class SellerProducts
    {

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public int StockCount { get; set; }
        public string StockCode { get; set; }
        public string BrandName { get; set; }
        public int VatRate { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }

        public List<ProductAttributes> Attributes { get; set; }
        public List<ProductCategorys> Categories { get; set; }
        //public SellerProductPrices Prices { get; set; }
        public List<ProductImages> Images { get; set; }
        public List<ProductGroups> ProductGroups { get; set; }
        public List<ProductDeliveryTypes> DeliveryTypes { get; set; }
    }

    public class ProductAttributes
    {
        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }
    }

    public class ProductCategorys
    {
        public string CategoryName { get; set; }
    }

    //public class SellerProductPrices
    //{
    //    public int VatRate { get; set; }
    //    public decimal ListPrice { get; set; }
    //    public decimal SalePrice { get; set; }
    //    public decimal TotalPrice { get; set; }
    //    public int InstallmentCount { get; set; }
    //}

    public class ProductImages
    {
        public string ImageUrl { get; set; }
    }

    public class ProductGroups
    {
        public Guid ProductId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
    public class ProductDeliveryTypes
    {
        public List<string> CityName { get; set; }
        public string DeliveryTypeName { get; set; }
    }
}
