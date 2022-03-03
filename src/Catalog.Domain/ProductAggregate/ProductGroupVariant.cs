using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductGroupVariant : Entity
    {
        public string ProductGroupCode { get; protected set; }
        public Guid AttributeId { get; protected set; }
        public Catalog.Domain.AttributeAggregate.Attribute Attribute { get; protected set; }

        protected ProductGroupVariant()
        {
        }

        public ProductGroupVariant(string productGroupCode, Guid attributeId) : this()
        {
            ProductGroupCode = productGroupCode;
            AttributeId = attributeId;
        }

        public void SetProductGroupVariant(Guid attributeId)
        {
            AttributeId = attributeId;
        }



    }
}
