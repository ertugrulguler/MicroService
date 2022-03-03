using System;

namespace Catalog.Domain.CategoryAggregate.ServiceModel
{
    public class ExcelDataModelCategoryAttributeMap
    {
        public Guid Id { get; set; }
        public Guid AttributeValueId { get; set; }
        public Guid CategoryAttributeId { get; set; }
        public bool IsActive { get; set; }

    }
}
