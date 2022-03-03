using Catalog.ApplicationService.Communicator.Campaign.Model;
using Framework.Core.Helpers.Abstract;
using Framework.Core.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Catalog.Domain.CouponAggregate;

namespace Catalog.ApplicationService.Communicator.Campaign
{
    public class CampaignCommunicator : ICampaignCommunicator
    {
        private readonly IHttpRequestHelper _httpRequestHelper;
        private readonly string _baseUrl;

        public CampaignCommunicator(IConfiguration configuration, IHttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
            _baseUrl = configuration["CampaignCommunicatorBaseUrl"];
        }


        public async Task<UseCouponResponse> GetCoupon(Guid sellerId, Guid productId)
        {
            var param = new HttpRequestParameter("CAT", $"{_baseUrl }/coupons/search/{sellerId}/{productId}", MethodBase.GetCurrentMethod());
            return await _httpRequestHelper.GetAsync<UseCouponResponse>(param);
        }

        public async Task<ResponseBase<List<CouponWithSellersResponse>>> GetCouponsSeller(List<Guid> productIds)
        {
            var param = new HttpRequestParameter("CAT", $"{_baseUrl}/coupons/searchByChannelWithProducts", productIds, MethodBase.GetCurrentMethod());
            return await _httpRequestHelper.PostAsync<ResponseBase<List<CouponWithSellersResponse>>>(param);
        }
    }
}