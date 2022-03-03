using System;

namespace Catalog.Domain.AttributeAggregate.ServiceModels
{
    public class ExcelDataModelAttributeValue
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string SeoName { get; set; }
    }
}
