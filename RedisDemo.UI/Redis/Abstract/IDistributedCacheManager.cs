namespace RedisDemo.UI.Redis.Abstract
{
	public interface IDistributedCacheManager
	{
		byte[] Get(string key);
		//byte olmasının sebebi:Idistuributedcache byte olarak tutuyor
		T Get<T>(string key);
		void Set(string key, object value);
		void Refresh(string key);
		bool Any(string key);
		void Remove(string key);
	}
}
