using Framework.Core.Model;
using System;

namespace Catalog.ApiContract.Response.Query.BasketQueries
{
    public class StockDetail
    {
        //public StockDetail AlternativeSellerStock { get; set; }
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
        public int StockCount { get; set; }
        public Price ListPrice { get; set; } //normal(tam) fiyat
        public Price SalePrice { get; set; } // satış fiyatı
        public string SellerName { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
