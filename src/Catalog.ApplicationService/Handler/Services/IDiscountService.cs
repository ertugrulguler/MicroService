using Catalog.Domain.CouponAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.Model;
using Framework.Core.Model.Enums;

namespace Catalog.ApplicationService.Handler.Services
{
    public interface IDiscountService
    {
        Task<CouponServiceResponse> GetDiscountResult(Guid sellerId, Guid productId, decimal salePrice, decimal listPrice,ChannelCode channel);
        Task<ResponseBase<List<CouponWithSellersResponse>>> GetCouponSellers(List<Guid> productIds);
    }
}