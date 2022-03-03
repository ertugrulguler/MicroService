using Framework.Core.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.ApplicationService.Communicator.Campaign.Model
{
    public class UseCouponResponse : ResponseBase<UseCoupon>
    {

    }


    public class UseCoupon
    {
        public Guid SellerId { get; set; }
        public Guid CouponId { get; set; }
        public Guid ProductId { get; set; }
        public string CouponName { get; set; }
        public CouponCodeDto CouponCodes { get; set; }
        public decimal MinAmount { get; set; }
        public decimal Discount { get; set; }
        public DiscountTypeEnum DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CouponCodeDto
    {
        public string CouponCode { get; set; }
        public int Count { get; set; }

    }

    public enum DiscountTypeEnum
    {
        [Display(Name = "P")]
        Percent = 1,
        [Display(Name = "A")]
        Amount = 2
    }
}