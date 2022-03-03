using Catalog.ApplicationService.Communicator.User.Model;
using Framework.Core.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Communicator.User
{
    public class UserCommunicator : IUserCommunicator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAppLogger _appLogger;

        public UserCommunicator(IHttpClientFactory httpClientFactory, IAppLogger appLogger)
        {
            _httpClientFactory = httpClientFactory;
            _appLogger = appLogger;
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request)
        {
            _appLogger.MethodEntry(request, MethodBase.GetCurrentMethod());
            using (var userHttpClient = _httpClientFactory.CreateClient("user"))
            {
                userHttpClient.DefaultRequestHeaders.Add("jwt", "topkapi");
                userHttpClient.DefaultRequestHeaders.Add("correlationId", "topkapi");

                //todo bir interceptor düşünülebilir
                var timer = new Stopwatch();
                timer.Start();

                var httpResponseMessage =
                    await userHttpClient.GetAsync("http://userapi.topkapi.com/user/detail/" + Guid.NewGuid());
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

                timer.Stop();

                _appLogger.MethodExit(readAsStringAsync, MethodBase.GetCurrentMethod(), timer.ElapsedMilliseconds,
                    httpResponseMessage.StatusCode.ToString());
            }

            return new GetUserResponse();
        }

        public bool IsUp()
        {
            return true;
        }
    }
}