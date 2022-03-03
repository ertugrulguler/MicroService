using Catalog.ApplicationService.Communicator.Campaign.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.CouponAggregate;
using Framework.Core.Model;

namespace Catalog.ApplicationService.Communicator.Campaign
{
    public interface ICampaignCommunicator
    {
        Task<UseCouponResponse> GetCoupon(Guid sellerId, Guid productId);
        Task<ResponseBase<List<CouponWithSellersResponse>>> GetCouponsSeller(List<Guid> productIds);
    }
}