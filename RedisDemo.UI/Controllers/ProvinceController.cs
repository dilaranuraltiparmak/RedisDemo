using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace RedisDemo.UI.Controllers
{
	public class ProvinceController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		public ProvinceController(IMemoryCache memoryCache)
		{
			_memoryCache=memoryCache;
		}

		public IActionResult GetAll()
		{
			string name = _memoryCache.GetOrCreate("employeeName", entry =>
			{
				entry.SetValue("Memory Cache ");
				return entry.Value.ToString();
			});

			return Content(name);
		}


		public IActionResult GetDataType()
		{
			Redis.Concrete.StackExchange ex = new Redis.Concrete.StackExchange();
			string result = ex.GetRedisValue();
			return Content(result);
		}
	}
}
