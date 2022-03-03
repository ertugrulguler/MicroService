using Framework.Core.Attribute;
using Framework.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Container.Decorator
{
    public class CacheHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse> where TResponse : ResponseBase
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IWebHostEnvironment _environment;

        public CacheHandler(IDistributedCache distributedCache, IWebHostEnvironment environment)
        {
            _distributedCache = distributedCache;
            _environment = environment;
        }

        public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_environment.IsDevelopment())
            {
                return await next();
            }
            var cachedAttribute = GetHandlerMethodInfo().GetCustomAttributes<CacheInfoAttribute>().FirstOrDefault();
            var isCachedQueryBase = request is CachedQuery;
            if (cachedAttribute == null || !isCachedQueryBase)
                return await next();
            var cachedQuery = request as CachedQuery;
            var cachedJson = await _distributedCache.GetStringAsync(cachedQuery.CacheKey, cancellationToken);
            if (cachedJson == null)
            {
                var result = await next();

                if (result.Success)
                {
                    var serializeResult = JsonSerializer.Serialize(result);
                    await _distributedCache.SetStringAsync(cachedQuery.CacheKey, serializeResult,
                        token: cancellationToken);
                }

                return result;
            }
            else
            {
                var response = JsonSerializer.Deserialize<TResponse>(cachedJson);
                response.FromCache = true;
                return response;
            }
        }
    }
}