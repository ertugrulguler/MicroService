namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class GetCategoryAndImageBySellerIdResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
    }
}
