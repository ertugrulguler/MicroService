using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.ProductAggregate
{
    public class SimilarProduct : Entity
    {
        public Guid ProductId { get; protected set; }
        public Guid SecondProductId { get; protected set; }

        protected SimilarProduct()
        {
        }

        public SimilarProduct(Guid productId, Guid secondProductId) : this()
        {
            ProductId = productId;
            SecondProductId = secondProductId;
        }

        public void SetSimilarProduct(Guid productId, Guid secondProductId)
        {
            ProductId = productId;
            SecondProductId = secondProductId;
        }
    }
}
