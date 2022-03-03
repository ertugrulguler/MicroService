using Catalog.ApiContract.Contract;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class CategoryResultForSeller
    {

        public CategoryForSellerDto Main { get; set; }

        public CategoryResultForSeller()
        {
            Main = new CategoryForSellerDto("Tüm Kategoriler");
        }
    }
}