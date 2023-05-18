using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RedisDemo.UI.Models.DAL;
using RedisDemo.UI.Models.Entities;

namespace RedisDemo.UI.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IDistributedCache _cache;
		public CategoryController(IDistributedCache cache)
		{
			_cache = cache;
		}
        /// <summary>
        /// Bu metod, bir önbellek (cache) kullanarak bir metni saklar ve daha sonra aynı sayfa istendiğinde bu önbellekten veriyi alır.Öncelikle myCachedData adlı bir önbellek anahtarı(cache key) tanımlanmıştır.Daha sonra _cache.GetString(cacheKey) yöntemi kullanılarak önbellekte bu anahtarla kaydedilen herhangi bir veri alınmaya çalışılır.Eğer bu anahtarla kaydedilen veri önbellekte yoksa cachedData değişkeni, mvc den gelen veri metni ile doldurulur ve _cache.SetString(cacheKey, cachedData) yöntemi kullanılarak bu metin önbelleğe kaydedilir.Sonuç olarak, return Content(cachedData) satırı ile önbellekten alınan veya yeni kaydedilen cachedData metni sayfa içinde görüntülenir.Bu sayede aynı sayfa her istendiğinde yeni bir veritabanı sorgusu veya veri hesaplama işlemi yapılmak yerine, veri önbellekte tutulduğu için daha hızlı bir şekilde yanıt verilir.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string cacheKey = "myCachedData";
            string cachedData = _cache.GetString(cacheKey);
            if (cachedData == null)
            {
				cachedData = "mvc den gelen veri";
				_cache.SetString(cacheKey, cachedData);
			}
			
			return Content(cachedData);
		}
        /// <summary>
        /// Bu metod, bir kategori listesi almak için önbellek (cache) kullanır. Öncelikle categoryList adında bir List<Category> nesnesi tanımlanır.Daha sonra categoryList önbellekte tutulmuş mu diye kontrol edilir.Eğer önbellekte varsa, Get metodu kullanılarak redisCategoryList değişkeni önbellekten alınır ve bu değer UTF-8 kodlama kullanılarak serializedCategoryList değişkeninde değiştirilir.Sonrasında bu seri halden deserialization işlemi yapılır ve categoryList değişkenine aktarılır.  Eğer önbellekte veri yoksa, CategoryDal().GetAll() metodu ile bir kategori listesi oluşturulur.Bu kategori listesi, JsonConvert.SerializeObject(categoryList) yöntemi ile JSON formatına dönüştürülür ve Encoding.UTF8.GetBytes(serializedCategoryList) yöntemi ile UTF-8 kodlama kullanılarak byte dizisine dönüştürülür.Daha sonra bu byte dizisi önbelleğe kaydedilir ve son olarak DistributedCacheEntryOptions sınıfı kullanılarak önbellek ayarları belirlenir ve Set metodu kullanılarak önbelleğe kaydedilir. Sonuç olarak, categoryList değişkeni view'e gönderilir ve eğer veri önbellekte varsa, CategoryDal().GetAll() metodundan veri çekilmesine gerek kalmadan önbellekten hızlı bir şekilde yanıt döndürülür. Veri önbellekte bulunmadığı durumda, veri veritabanından çekilerek önbelleğe kaydedilir ve sonraki isteklerde önbellek kullanılarak daha hızlı yanıt döndürülür.
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAll()
		{
			var cacheKey = "categoryList";
			string serializedCategoryList;
			var categoryList = new List<Category>();
			var redisCategoryList = _cache.Get(cacheKey);
			if (redisCategoryList != null)
			{
				serializedCategoryList = Encoding.UTF8.GetString(redisCategoryList);
				categoryList = JsonConvert.DeserializeObject<List<Category>>(serializedCategoryList);
			}
			else
			{
				categoryList = new CategoryDal().GetAll();
				serializedCategoryList = JsonConvert.SerializeObject(categoryList);
				redisCategoryList = Encoding.UTF8.GetBytes(serializedCategoryList);
				var options = new DistributedCacheEntryOptions()
					.SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
					.SetSlidingExpiration(TimeSpan.FromMinutes(3));
				_cache.Set(cacheKey, redisCategoryList, options);
			}

			return View(categoryList);
		}



		[NonAction]
		public void RemoveCache(string cacheKey)
		{
			_cache.Remove(cacheKey);
		}
	}
}
