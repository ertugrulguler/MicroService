using System;

namespace Catalog.Domain.CategoryAggregate.ServiceModel
{
    public class ExcelDataModelCategoryAttribute
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public bool IsFilter { get; set; }
        public int FilterOrder { get; set; }
        public bool IsVariantable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsListed { get; set; }
        public Guid Id { get; set; }
        public bool IsActive { get; set; }

    }
}
