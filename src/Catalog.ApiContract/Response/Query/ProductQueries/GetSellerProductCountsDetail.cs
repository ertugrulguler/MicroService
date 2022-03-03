namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetSellerProductCountsDetail
    {
        public ProductCountAndStock OnSale { get; set; }
        public ProductCountAndStock OnSold { get; set; }
    }
    public class ProductCountAndStock
    {
        public int ProductCount { get; set; }
        public int StockCount { get; set; }
    }
}
