using System.Collections.Generic;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ProductFilterList
    {
        public string ParentName { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public List<ProductFilter> Value { get; set; }
        public bool IsSearchField { get; set; }
        public string ParentSeoName { get; set; }
    }
}
