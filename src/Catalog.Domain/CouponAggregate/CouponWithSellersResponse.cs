using System;

namespace Catalog.Domain.CouponAggregate
{
    public class CouponWithSellersResponse
    {
        public int Count { get; set; }
        public Guid CouponId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
    }
}