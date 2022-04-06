using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        public IQueryable<Location> Location => _ctx.Location;


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

    }
}

