using Catalog.ApiContract.Response.Query.SearchQueries;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System.Collections.Generic;
namespace Catalog.ApiContract.Response.Query.ProductQueries
{
    public class GetProductsFilterQueryResult
    {
        public List<SelectedFilters> SelectedFilters { get; set; }
        public int TotalCount { get; set; }
        public List<ProductList> ProductList { get; set; }
        public List<ProductFilterList> ProductFilters { get; set; }
        public List<ProductFilterList> QuickFilteringList { get; set; }
        public List<OrderByListofObject> OrderList { get; set; }
        public MMActionV2 MaximumMobilAction { get; set; }
        public PageResponse PageResponse { get; set; }
        public IEnumerable<Breadcrumb> Breadcrumb { get; set; }
        public OrderBy OrderBy { get; set; }
    }

    public class MMActionV2
    {
        public string SearchType { get; set; }
        public DeeplinkData DeeplinkData { get; set; }
    }
}
