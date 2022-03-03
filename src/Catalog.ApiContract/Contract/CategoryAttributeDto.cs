using System;

namespace Catalog.ApiContract.Contract
{
    public class CategoryAttributeDto
    {
        public Guid CategoryId { get; set; }

        public Guid AttributeId { get; set; }

        public bool IsRequired { get; set; }

        public AttributeDto Attribute { get; set; }
    }
}
