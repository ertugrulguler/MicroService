using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.CategoryAggregate
{
    public class CategoryAttributeValueMap : Entity
    {
        public Guid CategoryAttributeId { get; protected set; }
        public Guid AttributeValueId { get; protected set; }

        protected CategoryAttributeValueMap()
        {
        }
        public CategoryAttributeValueMap(Guid categoryAttributeId, Guid attributeValueId) : this()
        {
            CategoryAttributeId = categoryAttributeId;
            AttributeValueId = attributeValueId;
        }
        public CategoryAttributeValueMap(Guid id, Guid categoryAttributeId, Guid attributeValueId, bool isActive) : this()
        {
            Id = id;
            CategoryAttributeId = categoryAttributeId;
            AttributeValueId = attributeValueId;
            IsActive = isActive;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
    }
}
