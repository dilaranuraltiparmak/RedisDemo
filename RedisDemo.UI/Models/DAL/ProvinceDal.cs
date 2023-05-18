using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RedisDemo.UI.Models.DAL.Context;
using RedisDemo.UI.Models.Entities;

namespace RedisDemo.UI.Models.DAL
{
	public class ProvinceDal
	{
		public List<Province> GetAll()
		{
			List<Province> list = null;
			using (TestContext db = new TestContext())
			{
				list = db.Provinces.ToList();
			}

			return list;
		}
	}
}
