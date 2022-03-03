using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.AttributeAggregate
{
    public class AttributeValue : Entity
    {
        public Guid? AttributeId { get; protected set; }
        public string Value { get; protected set; }
        public string Unit { get; protected set; }
        public int Order { get; protected set; }
        public string Code { get; protected set; }
        public string SeoName { get; protected set; }

        protected AttributeValue()
        {
        }
        public AttributeValue(Guid id, string value, string code, bool isActive, string seoName) : this()
        {
            Id = id;
            AttributeId = null;
            Value = value;
            Unit = null;
            Order = 1;
            Code = code;
            IsActive = isActive;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            SeoName = seoName;


        }
        public AttributeValue(Guid id, string value, string code, bool isActive, int order, string seoName) : this()
        {
            Id = id;
            AttributeId = null;
            Value = value;
            Unit = null;
            Order = order;
            Code = code;
            IsActive = isActive;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            SeoName = seoName;
        }
        public AttributeValue(Guid attributeId, string value, string unit, int order, string seoName) : this()
        {
            AttributeId = attributeId;
            Value = value;
            Unit = unit;
            Order = order;
            SeoName = seoName;
        }
        public void SetAttributeValue(Guid attributeId, string value, string unit, int order, string seoName)
        {
            AttributeId = attributeId;
            Value = value;
            Unit = unit;
            Order = order;
            SeoName = seoName;
        }
    }
}
