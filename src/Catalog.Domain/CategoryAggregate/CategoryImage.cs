using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.CategoryAggregate
{
    public class CategoryImage : Entity
    {
        public Guid CategoryId { get; protected set; }
        public string Name { get; protected set; }
        public string Url { get; protected set; }
        public string Description { get; protected set; }

        protected CategoryImage()
        {

        }
        public CategoryImage(Guid categoryId, string name, string url, string description) : this()
        {
            CategoryId = categoryId;
            Name = name;
            Url = url;
            Description = description;
        }
    }
}
