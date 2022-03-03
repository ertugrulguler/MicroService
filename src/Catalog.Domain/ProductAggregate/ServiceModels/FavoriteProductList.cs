using System;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class FavoriteProductsList
    {
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
