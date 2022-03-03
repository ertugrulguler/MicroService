using System;

namespace Catalog.Domain.AttributeAggregate.ServiceModels
{
    public class ExcelDataModelAttributeMap
    {
        public Guid Id { get; set; }
        public Guid AttributeId { get; set; }
        public Guid AttributeValueId { get; set; }
        public bool IsActive { get; set; }

    }
}
