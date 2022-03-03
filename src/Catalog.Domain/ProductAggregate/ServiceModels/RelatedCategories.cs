using Microsoft.EntityFrameworkCore;
using System;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    [Keyless]
    public class RelatedCategories
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool HasProduct { get; set; }
        public string Code { get; set; }
        public string SeoName { get; set; }
    }
}
