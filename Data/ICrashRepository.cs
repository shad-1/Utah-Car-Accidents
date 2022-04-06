using System;
using System.Linq;
using YeetCarAccidents.Models;

namespace YeetCarAccidents.Data
{
	public interface ICrashRepository
	{
		public IQueryable<Crash> Crashes { get; }
		public IQueryable<Location> Locations { get; }

		public void AddCrash(Crash c);
		public void UpdateCrash(Crash c);
		public void DeleteCrash(Crash c);

		public void AddLocation(Location l);
		public void UpdateLocation(Location l);
		public void DeleteLocation(Location l);
	}
}

