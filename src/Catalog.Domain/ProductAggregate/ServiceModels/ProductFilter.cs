namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ProductFilter : SelectedFilters
    {
        public string Value { get; set; }
        public string ValueDetail { get; set; }
    }
    public class SelectedFilters
    {
        public string SeoUrl { get; set; }
        public bool IsSelectedFilter { get; set; }
        public string Id { get; set; }
        public string FilterField { get; set; }
        public string Type { get; set; }
        public string SeoValue { get; set; }
    }
}
