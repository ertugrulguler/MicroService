using System;

namespace Catalog.ApiContract.Contract
{
    public class ProductAttributeDto
    {

        public Guid AttributeId { get; set; }

        public Guid AttributeValueId { get; set; }

        public bool IsVariantable { get; set; }
    }
}
