using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class FavoriteRepoProductList
    {
        public List<FavoriteProduct> List { get; set; }
        public int TotalCount { get; set; }
    }
}
