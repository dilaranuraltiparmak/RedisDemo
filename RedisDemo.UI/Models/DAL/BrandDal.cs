using System.Collections.Generic;
using System.Linq;
using RedisDemo.UI.Models.DAL.Context;
using RedisDemo.UI.Models.Entities;

namespace RedisDemo.UI.Models.DAL
{
	public class BrandDal
	{
		public bool Add(Brand brand)
		{
			bool result = false;
			using (var context = new TestContext())
			{
				context.Brands.Add(brand);
				result=context.SaveChanges()>0;
			}

			return result;
		}

		public List<Brand> GetAll()
		{
			List<Brand> brands=null;
			using (var context = new TestContext())
			{
				brands = context.Brands.ToList();
			}
			return brands;
		}
	}
}
