using Catalog.ApiContract.Contract;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.CategoryQueries
{
    public class CategoryResultForMobile
    {

        public CategoryModelForMobileDto Main { get; set; }
        public CategoryModelForMobileDto Virtual { get; set; }
        public List<CategoryModelForMobileDto> Other { get; set; }

        public CategoryResultForMobile()
        {
            Main = new CategoryModelForMobileDto("Tüm Kategoriler");
            Virtual = new CategoryModelForMobileDto("Kampanyalar");
            Other = new List<CategoryModelForMobileDto>();
        }
    }
}