using Microsoft.EntityFrameworkCore;
using System;

namespace Catalog.Domain.ValueObject.StoreProcedure
{
    [Keyless]
    public class AttributeFilter
    {
        public Guid AttributeId { get; set; }
        public string DisplayName { get; set; }
        public int AttributeOrder { get; set; }
        public Guid AttributeValueId { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Order { get; set; }
        public string SeoName { get; set; }
        public string AttributeSeoName { get; set; }
    }
}
