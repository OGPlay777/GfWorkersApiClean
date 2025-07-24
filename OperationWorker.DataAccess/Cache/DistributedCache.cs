using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OperationWorker.Infrastructure;
using Serilog.Core;
using StackExchange.Redis;
using System.Text;

namespace OperationWorker.DataAccess.Cache
{
    public static class DistributedCache
    {
        private static ILogger? _logger;
        public static void InitLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("DistributedCache");
        }

        private static readonly JsonSerializerSettings serializerSettings = new()
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateResolver()
        };

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
               .SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        }

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var objects = JsonConvert.SerializeObject(value);
            try
            {
                var taskResponse = cache.SetStringAsync(key, objects, options);
                return Task.FromResult(taskResponse);
            }
            catch (RedisConnectionException ex)
            {
                _logger?.LogWarning(ex, "Redis unaccessible while setting key '{Key}'", key);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to save cache in Redis with key '{Key}'", key);
                return Task.CompletedTask;
            }

        }

        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            value = default;

            try
            {
                var cachedObject = cache.GetString(key);
                if (cachedObject == null) return false;

                value = JsonConvert.DeserializeObject<T>(cachedObject);
                return true;
            }
            catch (RedisConnectionException ex)
            {
                _logger?.LogWarning("Redis error: '{ex.Message}'", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning("Redis error: '{ex.Message}'", ex.Message);
                return false;
            }
        }

        public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task, DistributedCacheEntryOptions? options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            }
            if (cache.TryGetValue(key, out T? value) && value is not null)
            {
                return value;
            }
            value = await task();
            if (value is not null)
            {
                await cache.SetAsync<T>(key, value, options);
            }
            return value;
        }
    }
}
