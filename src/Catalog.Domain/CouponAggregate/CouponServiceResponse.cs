namespace Catalog.Domain.CouponAggregate
{
    public class CouponServiceResponse
    {
        public decimal DiscountedAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public string Code { get; set; }
        public bool IsDiscounted { get; set; }
    }
}