using Catalog.Domain.Entities;

using System;

namespace Catalog.Domain.ProductAggregate
{
    public class FavoriteProduct : Entity
    {
        public Guid ProductId { get; protected set; }
        public Guid CustomerId { get; protected set; }

        protected FavoriteProduct()
        {

        }

        public FavoriteProduct(Guid productId, Guid customerId, bool isActive) : this()
        {
            ProductId = productId;
            CustomerId = customerId;
            IsActive = isActive;
        }

        public void SetFavoriteProduct(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
