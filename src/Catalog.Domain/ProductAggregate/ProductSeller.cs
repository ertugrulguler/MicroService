using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductSeller : Entity
    {
        public Guid ProductId { get; protected set; }
        public Guid SellerId { get; protected set; }
        public string StockCode { get; protected set; }
        public Guid CurrencyId { get; protected set; }
        public int DisplayOrder { get; protected set; }
        public decimal SalePrice { get; protected set; }
        private decimal _ListPrice;

        public decimal ListPrice
        {
            get
            {
                return _ListPrice - 1 <= SalePrice ? SalePrice : _ListPrice;
            }
            protected set
            {
                _ListPrice = value;
            }
        }
        private int _StockCount;

        public int StockCount
        {
            get
            {
                return _StockCount < 0 ? 0 : _StockCount;
            }
            protected set
            {
                _StockCount = _StockCount < 0 ? 0 : value;
            }
        }
        public Guid? DiscountId { get; protected set; }
        public int InstallmentCount { get; protected set; }

        protected ProductSeller()
        {

        }

        public ProductSeller(Guid productId, Guid sellerId, string stockCode, decimal listPrice, int stockCount, decimal salePrice, int installmentCount) : this()
        {
            ProductId = productId;
            SellerId = sellerId;
            StockCode = stockCode;
            ListPrice = listPrice;
            StockCount = stockCount;
            SalePrice = salePrice;
            InstallmentCount = installmentCount;

        }

        public void SetProductSeller(string stockCode, decimal listPrice, int stockCount, decimal salePrice)
        {
            StockCode = stockCode;
            ListPrice = listPrice;
            StockCount = stockCount;
            SalePrice = salePrice;
        }

        public void SetProductPriceAndStock(decimal salePrice, decimal listPrice, int stockCount)
        {
            SalePrice = salePrice;
            ListPrice = listPrice;
            StockCount = stockCount;

        }

        public void SetProductStock(int stockCount)
        {
            StockCount = stockCount;
        }
        public void SetProductPrice(decimal salePrice, decimal listPrice)
        {
            SalePrice = salePrice;
            ListPrice = listPrice;

        }

        public void SetStockCode(string stockCode) => StockCode = stockCode;
        public void SetStockCount(int stockCount) => StockCount = stockCount;
        public void SetListPrice(decimal listPrice) => ListPrice = listPrice;
        public void SetSalePrice(decimal salePrice) => SalePrice = salePrice;
        public void SetDiscountId(Guid? discountId) => DiscountId = discountId;
        public void SetInstallmentCount(int installmentCount) => InstallmentCount = installmentCount;
    }
}
