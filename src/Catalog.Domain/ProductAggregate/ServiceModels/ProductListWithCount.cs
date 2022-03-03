using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ProductListWithCount
    {
        public List<Product> ProductList { get; set; }
        public List<Product> AllProductList { get; set; }
        public int TotalCount { get; set; }
    }

    public class ProductListWithCountV2
    {
        public List<Product> ProductList { get; set; }
        public List<Product> AllProductList { get; set; }
        public int TotalCount { get; set; }
    }
}
