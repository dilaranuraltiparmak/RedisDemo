using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RedisDemo.UI.Models.DAL;
using RedisDemo.UI.Models.Entities;
using RedisDemo.UI.Redis.Abstract;
using RedisDemo.UI.Redis.Concrete;
using static System.Net.WebRequestMethods;

namespace RedisDemo.UI.Controllers
{
	public class BrandController : Controller
	{
		private readonly IDistributedCacheManager _cache;
		public BrandController(IDistributedCacheManager cache)
		{
			_cache = cache;
		}

		public IActionResult Add()
		{
			return View(new Brand());
		}


        /// <summary>
        /// Bu metod, bir marka nesnesi eklemek için kullanılır.Öncelikle, HTTP Post isteği ile gelen Brand nesnesi parametre olarak alınır.Daha sonra bu nesne, BrandDal().Add(brand) metodu kullanılarak veritabanına kaydedilir.Eğer işlem başarılı olursa, yani result değeri true dönerse, _cache nesnesi kullanılarak brandList adında önbellekte tutulan marka listesi silinir. Bu sayede sonraki isteklerde önbellekte tutulan eski veriler kullanılmaz ve güncel veriler kullanılır.En son olarak, RedirectToAction("GetAll") metodu kullanılarak tüm marka listesi sayfasına yönlendirme yapılır.
                /// </summary>
                /// <param name="brand"></param>
                /// <returns></returns>
     
		[HttpPost]
		public IActionResult Add(Brand brand)
		{
			var result = new BrandDal().Add(brand);
			if (result)
			{
				_cache.Remove("brandList");
			}
			return RedirectToAction("GetAll");
		}


        /// <summary>
        /// Bu metod, tüm markaların listelendiği bir sayfa için kullanılır.   Öncelikle, _cache nesnesi kullanılarak brandList adında önbellekte tutulan marka listesi kontrol edilir.Eğer bu liste önbellekte yoksa, BrandDal().GetAll() metodu kullanılarak veritabanından tüm markalar çekilir ve _cache nesnesi kullanılarak brandList adında önbelleğe alınır. Eğer önbellekte brandList adında bir liste varsa, bu liste _cache nesnesi kullanılarak alınır ve bir List<Brand> nesnesine atanır. En son olarak, tüm markalar View metodu kullanılarak bir görünüme aktarılır ve tarayıcıda görüntülenmek üzere gönderilir.Bu sayede sonraki isteklerde veritabanı bağlantısı olmadan önbellekte tutulan eski veriler kullanılabilir.
        /// </summary>
        /// <returns></returns>

        public IActionResult GetAll()
		{
			List<Brand> brands = null;

			if (!_cache.Any("brandList"))
			{
				brands=new BrandDal().GetAll();
				_cache.Set("brandList", brands);
			}
			else
			{
				brands = _cache.Get<List<Brand>>("brandList");
			}
			
			return View(brands);
		}
	}
}
