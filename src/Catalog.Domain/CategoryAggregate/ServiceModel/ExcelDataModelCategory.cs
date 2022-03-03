using System;

namespace Catalog.Domain.CategoryAggregate.ServiceModel
{
    public class ExcelDataModelCategory
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool IsLeaf { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CategoryParentId { get; set; }
        public bool IsActive { get; set; }
        public string SeoName { get; set; }
        public bool IsRequiredIdNumber { get; set; }
        public bool IsReturnable { get; set; }
    }
}
