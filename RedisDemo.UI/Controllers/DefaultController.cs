using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisDemo.UI.Redis.Concrete;

namespace RedisDemo.UI.Controllers
{
	public class DefaultController : Controller
	{
		public IActionResult Index()
		{
			var result = new Redis.Concrete.StackExchange().SetRedisValue();
			return View(result.ToList());

			//redisi veritabanı olarak kullanma--stackexchange
		}
	}
}
