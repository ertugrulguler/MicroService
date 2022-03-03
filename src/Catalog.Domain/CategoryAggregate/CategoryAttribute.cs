using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.CategoryAggregate
{
    public class CategoryAttribute : Entity
    {
        public Guid CategoryId { get; protected set; }
        public Guid AttributeId { get; protected set; }
        public int Order { get; set; }
        public bool IsRequired { get; protected set; }
        public bool IsVariantable { get; protected set; }
        public bool IsListed { get; protected set; }
        public bool IsFilter { get; protected set; }
        public int FilterOrder { get; protected set; }
        public Catalog.Domain.AttributeAggregate.Attribute Attribute { get; protected set; }

        protected CategoryAttribute()
        {
        }

        public CategoryAttribute(Guid id, Guid attributeId, Guid categoryId, bool isVariantable, bool isRequired, bool isActive, bool isFilter, int filterOrder, bool isListed) : this()
        {
            Id = id;
            AttributeId = attributeId;
            CategoryId = categoryId;
            IsVariantable = isVariantable;
            IsRequired = isRequired;
            IsFilter = isFilter;
            FilterOrder = filterOrder;
            IsListed = isListed;
            IsActive = isActive;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        public CategoryAttribute(Guid categoryId, Guid attributeId, bool isRequired, bool isVariantable, bool isListed) : this()
        {
            CategoryId = categoryId;
            AttributeId = attributeId;
            IsRequired = isRequired;
            IsVariantable = isVariantable;
            IsListed = isListed;
        }

        public void SetCategoryAttribute(Guid categoryId, Guid attributeId, bool isRequired, bool isVariantable, bool isListed, bool isActive)
        {
            CategoryId = categoryId;
            AttributeId = attributeId;
            IsRequired = isRequired;
            IsVariantable = isVariantable;
            IsListed = isListed;
            IsActive = isActive;
        }
        public void UpdateCategoryAttribute(bool isRequired, bool isVariantable, bool isListed, bool isActive)
        {
            IsRequired = isRequired;
            IsVariantable = isVariantable;
            IsListed = isListed;
            IsActive = isActive;
        }
        public void SetCategoryAttributeIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
