using System;
using AspNetCore6WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore6WebAPI.DbContexts
{
	public class CityInfoContext : DbContext
	{
		public DbSet<City> Cities { get; set; } = null!;
		public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("connectionstring");
            base.OnConfiguring(optionsBuilder);
        }
    }
}