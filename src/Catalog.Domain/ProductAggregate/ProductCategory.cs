using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class ProductCategory : Entity
    {
        public Guid ProductId { get; protected set; }
        public Guid CategoryId { get; protected set; }

        protected ProductCategory()
        {

        }

        public ProductCategory(Guid productId, Guid categoryId) : this()
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public void SetProductCategory(Guid productId, Guid categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}
