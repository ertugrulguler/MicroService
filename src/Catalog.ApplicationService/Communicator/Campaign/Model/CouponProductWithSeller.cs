using System;

namespace Catalog.ApplicationService.Communicator.Campaign.Model
{
    public class CouponProductWithSeller
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
    }
}