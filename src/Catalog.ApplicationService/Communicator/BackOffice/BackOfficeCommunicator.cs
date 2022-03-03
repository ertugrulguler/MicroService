using Catalog.ApplicationService.Communicator.BackOffice.Model;
using Framework.Core.Logging;
using Framework.Core.Model;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.BackOffice
{
    public class BackOfficeCommunicator : IBackOfficeCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;
        private static string _baseUrl;

        public BackOfficeCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
            _baseUrl = configuration["BackOfficeCommunicatorBaseUrl"];
        }


        public async Task<UpdatePriceAndInventoryApproveResponse> UpdatePriceAndInventoryApprove(UpdatePriceAndInventoryApproveRequest request)
        {
            var response = new UpdatePriceAndInventoryApproveResponse();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("backoffice"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/product/updatePriceAndInventoryApprove", content);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<UpdatePriceAndInventoryApproveResponse>(readAsStringAsync, options);
            }

            return response;
        }

        public async Task<ResponseBase<CategoryCompanyInstallmentResponse>> CategoryCompanyInstallment(CategoryCompanyInstallmentRequest request)
        {
            var response = new ResponseBase<CategoryCompanyInstallmentResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("backoffice"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/category/getCategoryCompanyInstallment", content);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                    timer.Stop();

                    _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                        httpResponseMessage.StatusCode.ToString());
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    response = JsonSerializer.Deserialize<ResponseBase<CategoryCompanyInstallmentResponse>>(readAsStringAsync, options);
                }
            }

            return response;
        }

        public async Task<ResponseBase<CategoryCompanyMileResponse>> CategoryCompanyMile(CategoryCompanyMileRequest request)
        {
            var response = new ResponseBase<CategoryCompanyMileResponse>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("backoffice"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/category/getCategoryCompanyMile", content);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<CategoryCompanyMileResponse>>(readAsStringAsync, options);
            }

            return response;
        }

        public async Task<ResponseBase<object>> DeleteCache(string key)
        {
            var response = new ResponseBase<object>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("backoffice"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var httpResponseMessage =
                    await userHttpClient.DeleteAsync(_baseUrl + "/cache/delete?Key=" + key);
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<object>>(readAsStringAsync, options);
            }

            return response;
        }



    }
}
