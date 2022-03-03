using Catalog.ApplicationService.Communicator.Merchant.Model;
using Framework.Core.Logging;
using Framework.Core.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Merchant
{
    public class MerhantCommunicator : IMerhantCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;
        private static string _baseUrl;

        public MerhantCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
            _baseUrl = configuration["MerchantCommunicatorBaseUrl"];
        }
        public async Task<ResponseBase<GetSellerFilterInfosResponse>> GetProductSellersWithFilters(GetSellerFilterInfosRequest request)
        {
            var response = new ResponseBase<GetSellerFilterInfosResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/seller/getProductSellersWithFilters", content);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<GetSellerFilterInfosResponse>>(readAsStringAsync, options);
            }
            return response;
        }

        public async Task<ResponseBase<GetSellerResponse>> GetSellerById(GetSellerRequest request)
        {
            var response = new ResponseBase<GetSellerResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage = await userHttpClient.GetAsync(_baseUrl + "/seller/getSeller" + "?UserId=" + request.SellerId);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<GetSellerResponse>>(readAsStringAsync, options);
            }
            return response;
        }

        public async Task<ResponseBase<GetSellerDeliveryResponse>> GetDeliveriesWithId(GetSellerDeliveryRequest request)
        {
            var response = new ResponseBase<GetSellerDeliveryResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/seller/getDeliveriesById", content);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<GetSellerDeliveryResponse>>(readAsStringAsync, options);
            }
            return response;
        }

        public async Task<ResponseBase<List<Guid>>> GetBannedSellers()
        {
            var response = new ResponseBase<List<Guid>>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                //  var content = JsonContent.Create(request);
                var httpResponseMessage = await userHttpClient.GetAsync(_baseUrl + "/seller/getBannedSellers" + "?UserId=");
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<List<Guid>>>(readAsStringAsync, options);
            }
            return response;
        }

        public async Task<ResponseBase<GetSellerBySeoNameResponse>> GetSellerBySeoName(string seoName)
        {
            var response = new ResponseBase<GetSellerBySeoNameResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var httpResponseMessage = await userHttpClient.GetAsync(_baseUrl + "/seller/getSellerBySeoName" + "?SeoName=" + seoName);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<GetSellerBySeoNameResponse>>(readAsStringAsync, options);
            }
            return response;
        }

        public async Task<ResponseBase<List<GetSellerDetailByIdsResponse>>> GetSellerDetailByIds(GetSellerDetailByIdsRequest request)
        {
            var response = new ResponseBase<List<GetSellerDetailByIdsResponse>>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("merchant"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);

                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/seller/getSellerDetailByIds", content);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<List<GetSellerDetailByIdsResponse>>>(readAsStringAsync, options);
            }

            return response;
        }
    }
}
