using System;

namespace Catalog.Domain.ValueObject
{
    public class RequestBasketInfo
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
    }
}
