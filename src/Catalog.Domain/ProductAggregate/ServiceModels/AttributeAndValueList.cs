using System;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class AttributeAndValueList
    {
        public Guid AttributeId { get; set; }
        public Guid AttributeValueId { get; set; }
    }
}
