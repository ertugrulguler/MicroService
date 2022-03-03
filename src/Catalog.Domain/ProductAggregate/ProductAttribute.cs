using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductAttribute : Entity
    {
        public Guid ProductId { get; protected set; }

        public Guid AttributeId { get; protected set; }

        public Guid AttributeValueId { get; protected set; }

        public bool IsVariantable { get; protected set; }

        protected ProductAttribute()
        {
        }

        public ProductAttribute(Guid productId, Guid attributeId, Guid attributeValueId, bool isVariantable) : this()
        {
            ProductId = productId;
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
            IsVariantable = isVariantable;
        }

        public void SetProductAttribute(Guid productId, Guid attributeId, Guid attributeValueId, bool isVariantable)
        {
            ProductId = productId;
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
            IsVariantable = isVariantable;
        }
    }
}
