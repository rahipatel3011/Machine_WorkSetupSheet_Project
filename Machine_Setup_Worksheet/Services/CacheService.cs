using Machine_Setup_Worksheet.Services.IServices;
using StackExchange.Redis;
using System.Text.Json;

namespace Machine_Setup_Worksheet.Services
{
    /// <summary>
    /// Service for caching data using Redis.
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        /// <summary>
        /// Initializes a new instance of the CacheService class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer.</param>
        public CacheService(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
        }

        /// <summary>
        /// Deletes the cached item associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the item to delete.</typeparam>
        /// <param name="key">The key associated with the item to delete.</param>
        /// <returns>true if the item was deleted; otherwise, false.</returns>
        public async Task<bool> Delete(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                return await _cacheDb.KeyDeleteAsync(key);
            }
            return false;
        }

        /// <summary>
        /// Retrieves the cached item associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the item to retrieve.</typeparam>
        /// <param name="key">The key associated with the item to retrieve.</param>
        /// <returns>The retrieved item if found; otherwise, the default value of <typeparamref name="T"/>.</returns>
        public async Task<T> Get<T>(string key)
        {
            string data = await _cacheDb.StringGetAsync(key);
            if (!String.IsNullOrEmpty(data))
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }

        /// <summary>
        /// Saves an item in the cache with the specified key and expiry time.
        /// </summary>
        /// <typeparam name="T">The type of the item to save.</typeparam>
        /// <param name="key">The key associated with the item to save.</param>
        /// <param name="value">The item to save.</param>
        /// <param name="expirytime">The expiry time of the cached item.</param>
        /// <returns>true if the item was saved; otherwise, false.</returns>
        public bool Save<T>(string key, T value, DateTimeOffset expirytime)
        {
            var expiryTime = expirytime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        }
    }
}
