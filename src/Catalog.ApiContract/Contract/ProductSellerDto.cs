using System;

namespace Catalog.ApiContract.Contract
{
    public class ProductSellerDto
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string StockCode { get; set; }
        public int StockCount { get; set; }
        public Guid CurrencyId { get; set; }
        public int VatRate { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
        public Guid? DiscountId { get; set; }
        public int InstallmentCount { get; set; }
    }
}
