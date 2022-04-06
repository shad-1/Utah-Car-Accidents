using System;
using System.Linq;
using YeetCarAccidents.Models;

namespace YeetCarAccidents.Data
{
    public class EFCrashRepository : ICrashRepository
	{
        private CrashContext _ctx { get; set; }

		public EFCrashRepository(CrashContext ctx)
		{
            _ctx = ctx;
		}


        public IQueryable<Crash> Crashes => _ctx.Crashes;
        public IQueryable<Location> Locations => _ctx.Locations;


        public async void AddCrash(Crash c)
        {
            try
            {
                _ctx.Crashes.Add(c);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void AddLocation(Location l)
        {
            try
            {
                _ctx.Locations.Add(l);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void DeleteCrash(Crash c)
        {
            try
            {
                _ctx.Crashes.Remove(c);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void DeleteLocation(Location l)
        {
            try
            {
                _ctx.Locations.Remove(l);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void UpdateCrash(Crash c)
        {
            try
            {
                _ctx.Crashes.Update(c);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void UpdateLocation(Location l)
        {
            try
            {
                _ctx.Locations.Update(l);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

