using Catalog.ApplicationService.Communicator.Search.Model;
using Framework.Core.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;


namespace Catalog.ApplicationService.Communicator.Search
{
    public class SearchCommunicator : ISearchCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;
        private static string _baseUrl;

        public SearchCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
            _baseUrl = configuration["SearchBaseUrl"];
        }
        public async Task<DidYouMeanResponse> DidYouMean(DidYouMeanRequest request)
        {
            var response = new DidYouMeanResponse();
            _appLogger.MethodEntry(request, MethodBase.GetCurrentMethod());
            try
            {
                using (var userHttpClient = _httpClientFactory.CreateClient())//Sor!!!!!!!!!!!!!!!!!!!!!!!!!!
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    var content = JsonContent.Create(request);

                    //userHttpClient.DefaultRequestHeaders.("content-type", "application/json");
                    userHttpClient.DefaultRequestHeaders.Add("cbot-token", "jO5fMwttPHctIUTxdU9jr1E4LrGB2LRe");

                    var httpResponseMessage = await userHttpClient.PostAsync(_baseUrl, content);
                    var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                    //userHttpClient.DefaultRequestHeaders.Remove("content-type");
                    userHttpClient.DefaultRequestHeaders.Remove("cbot-token");

                    timer.Stop();

                    _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                        httpResponseMessage.StatusCode.ToString());
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    response = JsonSerializer.Deserialize<DidYouMeanResponse>(readAsStringAsync, options);
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public async Task<SearchResponse> Search(SearchRequest request)
        {
            var response = new SearchResponse();
            _appLogger.MethodEntry(request, MethodBase.GetCurrentMethod());
            try
            {
                using (var userHttpClient = _httpClientFactory.CreateClient())//Sor!!!!!!!!!!!!!!!!!!!!!!!!!!
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    var content = JsonContent.Create(request);

                    //userHttpClient.DefaultRequestHeaders.("content-type", "application/json");
                    userHttpClient.DefaultRequestHeaders.Add("cbot-token", "jO5fMwttPHctIUTxdU9jr1E4LrGB2LRe");

                    var httpResponseMessage = await userHttpClient.PostAsync(_baseUrl, content);
                    var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                    //userHttpClient.DefaultRequestHeaders.Remove("content-type");
                    userHttpClient.DefaultRequestHeaders.Remove("cbot-token");

                    timer.Stop();

                    _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                        httpResponseMessage.StatusCode.ToString());
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    response = JsonSerializer.Deserialize<SearchResponse>(readAsStringAsync);
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }
    }
}
