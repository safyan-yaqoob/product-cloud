using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernal.Caching
{
	public class RedisAppCache : IAppCache
	{
		private readonly IDistributedCache _cache;

		public RedisAppCache(IDistributedCache cache)
		{
			_cache = cache;
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
		{
			var options = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
			};

			var json = JsonSerializer.Serialize(value);
			await _cache.SetStringAsync(key, json, options);
		}

		public async Task<T?> GetAsync<T>(string key)
		{
			var json = await _cache.GetStringAsync(key);
			return json is null ? default : JsonSerializer.Deserialize<T>(json);
		}

		public async Task RemoveAsync(string key)
		{
			await _cache.RemoveAsync(key);
		}
	}
}
