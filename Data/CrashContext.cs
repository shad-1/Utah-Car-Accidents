using System;
using Microsoft.EntityFrameworkCore;
using YeetCarAccidents.Models;

namespace YeetCarAccidents.Data
{
	public class CrashContext: DbContext
	{
		public CrashContext(DbContextOptions<CrashContext> options) : base(options)
		{
		}

		public DbSet<Crash> Crashes { get; set; }
		public DbSet<Location> Locations { get; set; }

	}
}

