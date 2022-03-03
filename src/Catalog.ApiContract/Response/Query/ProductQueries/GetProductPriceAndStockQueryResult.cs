namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductPriceAndStockQueryResult
    {
        public int StockCount { get; set; }
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}