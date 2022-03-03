using Framework.Core.Model.Enums;

namespace Catalog.ApplicationService.Helper
{
    public class CouponDiscountHelper
    {
        /// <summary>
        /// ileride product channelCode ye göre farklı indirimler uygulanabilir. Metotların içerisine ekleme yapılabilir.
        /// </summary>
        /// <param name="productChannelCode"></param>
        /// <param name="salePrice"></param>
        /// <param name="count"></param>
        /// <param name="couponDiscount"></param>
        /// <returns></returns>
        public static decimal SetFixDiscountAmount(ChannelCode productChannelCode, decimal salePrice,  decimal couponDiscount)
        {
            if (productChannelCode == ChannelCode.IsCep)
            {
                return salePrice  - couponDiscount;
            }

            return salePrice;
        }


        public static decimal SetPercentDiscountAmount(ChannelCode productChannelCode, decimal salePrice,  decimal couponDiscount)
        {
            if (productChannelCode == ChannelCode.IsCep)
            {
                return salePrice  * (1 - couponDiscount / 100);
            }

            return salePrice;
        }
    }
}