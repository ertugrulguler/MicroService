using Catalog.ApplicationService.Communicator.Campaign;
using Catalog.ApplicationService.Communicator.Campaign.Model;
using Catalog.ApplicationService.Helper;
using Catalog.Domain.CouponAggregate;
using Catalog.Domain.Enums;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.Model;


namespace Catalog.ApplicationService.Handler.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ICampaignCommunicator _campaignCommunicator;
        public DiscountService(ICampaignCommunicator campaignCommunicator)
        {
            _campaignCommunicator = campaignCommunicator;
        }

        public async Task<CouponServiceResponse> GetDiscountResult(Guid sellerId, Guid productId, decimal salePrice, decimal listPrice, ChannelCode channel)
        {
            var discountedAmount = 0m;
            var discountRate = 0m;
            if (channel == ChannelCode.IsCep) //işcep için ayrı bir servise gidiyor
            {
                var couponResponse = await _campaignCommunicator.GetCoupon(sellerId, productId);
                if (couponResponse.Data == null)
                    return new CouponServiceResponse() { DiscountedAmount = salePrice, IsDiscounted = false };

                var discount = couponResponse.Data.Discount;
                var discountType = couponResponse.Data.DiscountType;

                if (discountType == DiscountTypeEnum.Amount)
                {
                    discountedAmount = CouponDiscountHelper.SetFixDiscountAmount(channel, salePrice, discount);
                    discountRate = ArrangeDiscountRate(discountedAmount, listPrice); //toplam liste fiyatı 
                }
                else
                {
                    discountedAmount = CouponDiscountHelper.SetPercentDiscountAmount(channel, salePrice, discount);
                    discountRate = (int)discount;
                }
                return new CouponServiceResponse() { DiscountedAmount = discountedAmount, DiscountRate = discountRate, IsDiscounted = true };

            }
            return new CouponServiceResponse() { DiscountedAmount = salePrice, IsDiscounted = false };

        }

        public async Task<ResponseBase<List<CouponWithSellersResponse>>> GetCouponSellers(List<Guid> productIds)
        {
            return await _campaignCommunicator.GetCouponsSeller(productIds);
        }

        private static int ArrangeDiscountRate(decimal salePrice, decimal listPrice)
        {
            if (listPrice == 0)
                return 0;
            return (int)((listPrice - salePrice) * 100 / listPrice);
        }
    }
}