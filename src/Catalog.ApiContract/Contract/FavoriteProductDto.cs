using System;

namespace Catalog.ApiContract.Contract
{
    public class FavoriteProductDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public bool IsActive { get; set; }
    }
}
