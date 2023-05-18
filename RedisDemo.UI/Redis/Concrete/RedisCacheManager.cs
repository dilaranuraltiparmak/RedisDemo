using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisDemo.UI.Redis.Abstract;

namespace RedisDemo.UI.Redis.Concrete
{
	public class RedisCacheManager : IDistributedCacheManager
	{
		private readonly IDistributedCache _cache;

		public RedisCacheManager(IDistributedCache redisServer)
		{
			_cache = redisServer;
		}

		public T Get<T>(string key)
		{
			var utf8String = Encoding.UTF8.GetString(Get(key));
			var result = JsonConvert.DeserializeObject<T>(utf8String);
			return result;
		}

		public byte[] Get(string key)
		{
			return _cache.Get(key);
		}

		public void Set(string key, object value)
		{
			////byte array olarak tutuyor cache veriyi tutar
			var serializedObject = JsonConvert.SerializeObject(value);
			var utf8String = Encoding.UTF8.GetBytes(serializedObject);
			var options = new DistributedCacheEntryOptions()
				.SetAbsoluteExpiration(DateTime.Now.AddMinutes(15));
			_cache.Set(key, utf8String,options);
		
		}

		public void Refresh(string key)
		{
			_cache.Refresh(key);
		}

		public bool Any(string key)
		{
			return _cache.Get(key) != null;
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}
	}
}
