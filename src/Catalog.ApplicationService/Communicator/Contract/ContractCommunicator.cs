using Catalog.ApplicationService.Communicator.Contract.Model;
using Framework.Core.Logging;
using Framework.Core.Model;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.Contract
{
    public class ContractCommunicator : IContractCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;
        private static string _baseUrl;

        public ContractCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
            _baseUrl = configuration["ContractCommunicatorBaseUrl"];
        }
        public async Task<ResponseBase<object>> TemplatePreview(TemplatePreviewRequest request)
        {
            var response = new ResponseBase<object>();
            _appLogger.MethodEntry(null, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("contract"))
            {
                var timer = new Stopwatch();
                timer.Start();
                var content = JsonContent.Create(request);
                var httpResponseMessage =
                    await userHttpClient.PostAsync(_baseUrl + "/template/preview", content);
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
