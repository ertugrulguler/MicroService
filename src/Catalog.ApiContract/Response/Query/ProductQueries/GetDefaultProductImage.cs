namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetDefaultProductImage
    {
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductSeoUrl { get; set; }
        public string SellerName { get; set; }
    }
}