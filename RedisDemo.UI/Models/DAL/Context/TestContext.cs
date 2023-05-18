using Microsoft.EntityFrameworkCore;
using RedisDemo.UI.Models.Entities;

namespace RedisDemo.UI.Models.DAL.Context
{
	public class TestContext:DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-B159TJH;Database=RedisDemo;Trusted_Connection=True;");
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Province> Provinces { get; set; }
	}
}
