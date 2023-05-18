using StackExchange.Redis;
using System;

namespace RedisDemo.UI.Redis.Concrete
{
	public class StackExchange
	{
		//redisi veritabanı olarak nasıl tutacağımızı test ediyoruz
		public string GetRedisValue()
		{
			var cachedKey = "veritipitest";
			string value;

			using (var redis = ConnectionMultiplexer.Connect("localhost:6379"))
			{
				//baglanti icin
				IDatabase db = redis.GetDatabase();

				if (!db.KeyExists(cachedKey))
				{
					value = DateTime.Now.ToString();
					db.StringSet(cachedKey, value, TimeSpan.FromSeconds(18));
				}
				else
				{
					value = db.StringGet(cachedKey);
				}
			}

			return value;
		}

		public RedisValue[] SetRedisValue()
		{

			//liste şeklinde oluşturma
			//array şeklinde döner liste şeklinde alıyoruz
			var cachedKey = "listetesti";
			RedisValue[] value=null;
			using (var redis = ConnectionMultiplexer.Connect("localhost:6379"))
			{
				IDatabase db = redis.GetDatabase();
				if (!db.KeyExists(cachedKey))
				{
					db.ListRightPush(cachedKey, "dizinin 1. elemanı");
					db.ListRightPush(cachedKey, "dizinin 2. elemanı");
					db.ListRightPush(cachedKey, "dizinin 3. elemanı");
					
				}
				else
				{
					value = db.ListRange(cachedKey,start:0,stop:-1);
				}
			}
			return value;
		}

	}
}
