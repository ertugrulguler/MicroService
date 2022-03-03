using Catalog.Domain.ProductAggregate.ServiceModels;
using System.Collections.Generic;

namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductListAndFilterBySellerQueryResult
    {
        public List<SelectedFilters> SelectedFilters { get; set; }
        public int TotalCount { get; set; }
        public List<ProductList> ProductList { get; set; }
        public List<OrderByListofObject> OrderList { get; set; }
        public List<ProductFilterList> ProductFilters { get; set; }
        public MMAction MaximumMobilAction { get; set; }

    }
}
