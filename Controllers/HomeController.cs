using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YeetCarAccidents.Models;
using YeetCarAccidents.Models.ViewModels;
using YeetCarAccidents.Data;
using CoordinateSharp;

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
        [Route("Dashboard/{County}")]
        [Route("Home/Dashboard/{County}")]
        [Route("Dashboard/{County}/{pageNum}")]
        [Route("Home/Dashboard/{County}/{pageNum}")]
        [HttpGet]
        public async Task<IActionResult> Dashboard(string County = "All", int pageNum = 1)
        {
            const int cardsPerPage = 10;
            var crashes = await _repo.Crashes
                .OrderByDescending(crash => crash.DateTime)
                .Skip((pageNum -1) * cardsPerPage)
                .Take(cardsPerPage)
                .Include("Location")
                .ToListAsync();

            var location = await _repo.Location.Take(cardsPerPage * 3).ToListAsync(); //todo: add location filtering

            var vm = new DashboardViewModel
            {
                Crashes = crashes,
                Locations = location,
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
        //TEST
        [Route("SuperSecret")]
        [Route("Home/SuperSecret")]
        [HttpGet]
        [Authorize]
        public IActionResult SuperSecret()
        {
            return View();
        }
        //COOKIE SECTION

        [Route("MapCrash")]
        [Route("Home/MapCrash")]
        [HttpGet]
        public async Task<IActionResult> MapCrash(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            //UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
            UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, (double) crash.Location.Longitude, (double)crash.Location.Latitude);
            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

            return View(c);
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
        public async Task<IActionResult> Edit(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View("CrashChange", crash);
        }

        [Route("Home/Edit")]
        [HttpPost]
        public IActionResult Edit(Crash c)
        {
            _repo.UpdateCrash(c);
            return RedirectToAction("Admin");
        }

        [Route("Delete")]
        [Route("Home/Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(int crashid)
        {
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View(crash);
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(Crash crash)
        {
            var c = await _repo.Crashes.SingleAsync(x => x.CrashId == crash.CrashId);
            _repo.DeleteCrash(c);
            return RedirectToAction("Admin");
        }
        
    }
}
