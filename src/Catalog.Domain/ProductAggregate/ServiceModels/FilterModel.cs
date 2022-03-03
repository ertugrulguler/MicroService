namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class FilterModel
    {
        public string FilterField { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public bool IsSelectedFilter { get; set; }
        public string SeoUrl { get; set; }

    }
}
