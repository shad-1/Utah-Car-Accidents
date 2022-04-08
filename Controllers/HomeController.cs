using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YeetCarAccidents.Models;
using YeetCarAccidents.Models.ViewModels;
using YeetCarAccidents.Data;
using CoordinateSharp;
using System.Reflection;
using System.Collections.Generic;

namespace YeetCarAccidents.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private ICrashRepository _repo;

        public HomeController(ILogger<HomeController> logger, ICrashRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [Route("~/")]
        [Route("Home")]
        [Route("Index")]
        [Route("Home/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Dashboard")]
        [Route("Home/Dashboard")]
        [Route("Dashboard{pageNum}")]
        [Route("Home/Dashboard/{pageNum}", Name = "Dashboard")]
        [Route("Dashboard{pageNum}/{filter?}")]
        [Route("Home/Dashboard/{pageNum}/{filter?}")]
        [HttpGet]
        public async Task<IActionResult> Dashboard(int pageNum = 1, FilterInfo filter = null)
        {
            const int cardsPerPage = 10;
            var crashes = new List<Crash>();
            ViewBag.CurrentPage = pageNum;
            ViewBag.Filter = filter;
            bool isAdmin = HttpContext.User.IsInRole("Writer");
            ViewBag.isAdmin = isAdmin;


            if (filter is null)
            {
                crashes = await _repo.Crashes
                    .Include("Location")
                    .OrderByDescending(crash => crash.DateTime)
                    .Skip((pageNum - 1) * cardsPerPage)
                    .Take(cardsPerPage)
                    .ToListAsync();
            }
            else // We have a filter!
            {
                crashes = await _repo.Crashes
                    .Take(500)
                    .Include("Location")
                    .OrderByDescending(crash => crash.DateTime)
                    .ToListAsync();

                if (filter.County != null)
                    crashes = crashes
                    .Where(c => c.Location.County != null && c.Location.County == filter.County)
                    .ToList();

                if (filter.City != null)
                    crashes = crashes
                    .Where(c => c.Location.City != null && c.Location.City == filter.City)
                    .ToList();

                if (filter.Month != null)
                    crashes = crashes
                    .Where(c => c.DateTime != null && c.DateTime.Value.Month == filter.Month)
                    .ToList();

                if (filter.Year != null)
                    crashes = crashes
                    .Where(c => c.DateTime != null && c.DateTime.Value.Year == filter.Year)
                    .ToList();

                if (filter.booleans != null)
                    foreach(var b in filter.booleans)
                    {
                        crashes = crashes.Where(c => c.GetType().GetProperty(b).GetValue(c).Equals(true)).ToList();
                    }

                crashes = crashes.AsQueryable()
                    .Skip((pageNum - 1) * cardsPerPage)
                    .Take(cardsPerPage).ToList();
            }
            //var location = await _repo.Location.Take(cardsPerPage * 3).AsNoTracking().ToListAsync(); //todo: add location filtering

            // Loop through true properties on each crash and add them to the Tags
            foreach (Crash crash in crashes)
            {
                if (crash.Tags == null)
                    crash.Tags = new List<string>();

                foreach (PropertyInfo prop in crash.GetType().GetProperties())
                {
                    if (prop.PropertyType == typeof(bool?))
                    {
                        var value = prop.GetValue(crash);
                        if (value.Equals(true))
                        {
                            crash.Tags.Add(prop.Name);
                        }
                    }
                }
            }
            // End property tags

            var vm = new DashboardViewModel
            {
                Crashes = crashes,
                //Locations = location,
                Filter = filter ?? new FilterInfo(),
                PageInfo = new PageInfo
                {
                    Items = crashes.Count(),
                    PerPage = cardsPerPage,
                    CurrentPage = pageNum
                }
            };

            return View(vm);
        }
        [Route("Privacy")]
        [Route("Home/Privacy")]
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Error")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //*************************************************DELETE BEFORE DEPLOYMENT*****************************************
        //TEST
        [Route("SuperSecret")]
        [Route("Home/SuperSecret")]
        [HttpGet]
        public IActionResult SuperSecret()
        {
            bool isAdmin = HttpContext.User.IsInRole("Writer");
            ViewBag.isAdmin = isAdmin;
            return View();
        }
        //******************************************************************************************************************
        //COOKIE SECTION

        [Route("MapCrash")]
        [Route("Home/MapCrash")]
        [HttpGet]
        public async Task<IActionResult> MapCrash(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 12, (double)crash.Location.Longitude, (double)crash.Location.Latitude);
            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
            var lati = c.Latitude.ToDouble();
            var longit = c.Longitude.ToDouble();

            var murli = $"https://maps.googleapis.com/maps/api/staticmap?center=40.758701,-111.876183&zoom=8&size=800x800&key=AIzaSyALwDEe-8OduETgZsZ1xQ9RXTAbqbaYEbY&markers={lati},{longit}";

            var mvm = new MapsViewModel
            {
                murl = murli
            };
            return View(mvm);
        }


        [Route("Privacy")]
        [Route("Home/Admin")]
        [HttpGet]
        public async Task<IActionResult> Admin()
        {

            var crashes = await _repo.Crashes.Include("Location").Take(15).ToListAsync();
            return View(crashes);

            //var crashes = await _repo.Crashes.Take(15).ToListAsync();
            //var c = new AdminViewModel
            //{
            //    Crashes = crashes
            //};
            //return View(c);
        }

        [Route("Details")]
        [Route("Home/Details")]
        [HttpGet]
        public async Task<IActionResult> Details(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View("Details", crash);

        }

        [Route("CrashChange")]
        [Route("Home/CrashChange")]
        [HttpGet]
        public async Task<IActionResult> CrashChange()
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            return View();
        }


        [Route("Home/CrashChange")]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CrashChange(Crash c)
        {
            if (ModelState.IsValid)
            {
                var crashes = await _repo.Crashes.Take(100000).ToListAsync();
                var max = 0;
                foreach(Crash crash in crashes)
                {
                    if (max < crash.CrashId)
                    {
                        max = crash.CrashId;
                    }
                }
                c.CrashId = max + 1;
                _repo.AddCrash(c);
                return View("Confirmation", c);
            }
            else
            {
                ViewBag.Location = await _repo.Location.ToListAsync();
                return View();
            }
        }

        [Route("Edit")]
        [Route("Home/Edit")]
        [HttpGet]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Edit(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View("CrashChange", crash);
        }

        [Route("Home/Edit")]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public IActionResult Edit(Crash c)
        {
            _repo.UpdateCrash(c);
            return RedirectToAction("Admin");
        }

        [Route("Delete")]
        [Route("Home/Delete")]
        [HttpGet]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(int crashid)
        {
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View(crash);
        }

        [Route("Delete")]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(Crash crash)
        {
            var c = await _repo.Crashes.SingleAsync(x => x.CrashId == crash.CrashId);
            _repo.DeleteCrash(c);
            return RedirectToAction("Admin");
        }

 
        [Route("Home/Causes")]
        [HttpGet]
        public IActionResult Causes()
        {
            return View();
        }

        [Route("Home/Tableau")]
        [HttpGet]
        public IActionResult Tableau()
        {
            return View();
        }
    }
}
