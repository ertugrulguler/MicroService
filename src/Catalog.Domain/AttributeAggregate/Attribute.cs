using Catalog.Domain.Entities;

using System;
using System.Collections.Generic;

namespace Catalog.Domain.AttributeAggregate
{
    public class Attribute : Entity
    {
        public string Name { get; protected set; }
        public string Code { get; protected set; }
        public string DisplayName { get; protected set; }
        public string Description { get; protected set; }
        public string SeoName { get; protected set; }

        private readonly List<AttributeValue> _attributeValues;
        public IReadOnlyCollection<AttributeValue> AttributeValues => _attributeValues;

        private readonly List<AttributeMap> _attributeMaps;
        public IReadOnlyCollection<AttributeMap> AttributeMaps => _attributeMaps;

        protected Attribute()
        {
        }
        public Attribute(Guid id, string name, string code, bool isActive, string seoName) : this()
        {
            Id = id;
            Name = name;
            DisplayName = name;
            Description = null;
            Code = code;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            IsActive = isActive;
            SeoName = seoName;
        }
        public Attribute(string name, string displayName, string description, string seoName) : this()
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            SeoName = seoName;
            _attributeValues = new List<AttributeValue>();
            _attributeMaps = new List<AttributeMap>();
        }
        public void SetAttribute(string name, string displayName, string description, string seoName)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            SeoName = seoName;
        }
    }
}