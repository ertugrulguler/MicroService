using Framework.Core.Model;
using System;

namespace Catalog.ApiContract.Response.Query.BasketQueries
{
    public class BasketDetail
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public Guid CategoryId { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public string ProductImageUrl { get; set; }
        public string VariantOptionDisplay { get; set; }
        public Price ListPrice { get; set; } //normal(tam) fiyat
        public Price SalePrice { get; set; } // satış fiyatı
        public decimal ExternalDiscountAmount { get; set; } // satış fiyatı
        public Price VatPrice { get; set; }
        public decimal Desi { get; set; }
        public int VatRate { get; set; }
        public int StockCount { get; set; }
        public string StockCode { get; set; }
        public bool UseMile { get; set; }
        public Price MinRequiredAmountForMile { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public bool? IsReturnable { get; set; }
    }
}
