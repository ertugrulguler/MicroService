using Catalog.ApiContract.Contract;
using Catalog.ApplicationService.Communicator.Parameter.Model;
using Framework.Core.Logging;
using Framework.Core.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Parameter
{
    public class ParameterCommunicator : IParameterCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;
        private static string _baseUrl;

        public ParameterCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
            _baseUrl = configuration["ParameterCommunicatorBaseUrl"];
        }

        public async Task<ResponseBase<List<IconResponse>>> GetBadges()
        {
            var response = new ResponseBase<List<IconResponse>>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("parameter"))
            {
                var timer = new Stopwatch();
                timer.Start();

                var httpResponseMessage =
                    await userHttpClient.GetAsync(_baseUrl + "/icon/getIcon?batchCode=0");
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<List<IconResponse>>>(readAsStringAsync, options);
            }

            return response;
        }


        public async Task<ResponseBase<List<ProductChannelDto>>> GetProductChannelList()
        {
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using var userHttpClient = _httpClientFactory.CreateClient("parameter");
            var timer = new Stopwatch();
            timer.Start();

            var httpResponseMessage =
                await userHttpClient.GetAsync(_baseUrl + "/common/getChannelList");
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

            timer.Stop();

            _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                httpResponseMessage.StatusCode.ToString());
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var response = JsonSerializer.Deserialize<ResponseBase<List<ProductChannelDto>>>(readAsStringAsync, options);

            return response;
        }
        public async Task<ResponseBase<List<CityListResponse>>> GetCities()
        {
            var response = new ResponseBase<List<CityListResponse>>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("parameter"))
            {
                var timer = new Stopwatch();
                timer.Start();

                var httpResponseMessage =
                    await userHttpClient.GetAsync(_baseUrl + "/address/cities");
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                response = JsonSerializer.Deserialize<ResponseBase<List<CityListResponse>>>(readAsStringAsync, options);
            }

            return response;
        }
    }
}
