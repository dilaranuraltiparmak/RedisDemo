using System.Collections.Generic;
using System.Linq;
using RedisDemo.UI.Models.DAL.Context;
using RedisDemo.UI.Models.Entities;

namespace RedisDemo.UI.Models.DAL
{
	public class CategoryDal
	{
		public List<Category> GetAll()
		{
			List<Category> list = null;
			using (TestContext db = new TestContext())
			{
				list = db.Categories.ToList();
			}

			return list;
		}

		public void Add()
		{
			using (TestContext db = new TestContext())
			{
				for (int i = 5; i < 1000; i++)
				{
					db.Add(new Category()
					{
						Name = $"Test Cat {i}"
					});
				}

				db.SaveChanges();
			}
		}
	}
}
