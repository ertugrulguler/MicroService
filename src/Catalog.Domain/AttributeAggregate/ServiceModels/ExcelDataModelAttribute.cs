using System;

namespace Catalog.Domain.AttributeAggregate.ServiceModels
{
    public class ExcelDataModelAttribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string SeoName { get; set; }
    }
}
