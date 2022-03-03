using System;

namespace Catalog.Domain.ProductAggregate.ServiceModels
{
    public class ProductFavorite
    {
        public Product Product { get; set; }
        public Guid FavoriteProductId { get; set; }
    }
}
