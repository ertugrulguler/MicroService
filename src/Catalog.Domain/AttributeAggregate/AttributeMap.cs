using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.AttributeAggregate
{
    public class AttributeMap : Entity
    {
        public Guid AttributeId { get; protected set; }
        public Guid AttributeValueId { get; protected set; }

        protected AttributeMap()
        {
        }
        public AttributeMap(Guid attributeId, Guid attributeValueId) : this()
        {
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
        }
        public AttributeMap(Guid id, Guid attributeId, Guid attributeValueId, bool isActive) : this()
        {
            Id = id;
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            IsActive = isActive;
        }
    }
}
