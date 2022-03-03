using System;

namespace Catalog.Domain.CategoryAggregate.ServiceModel
{
    public class CategoryIdAndNameforProducts
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
