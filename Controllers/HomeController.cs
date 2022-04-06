using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YeetCarAccidents.Models;
using YeetCarAccidents.Models.ViewModels;
using YeetCarAccidents.Data;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard(int pageNum = 1)
        {
            const int cardsPerPage = 10;
            var crashes = _repo.Crashes.ToList();
            var locations = _repo.Locations.ToList(); //todo: add location filtering

            var vm = new DashboardViewModel
            {
                Crashes = crashes,
                Locations = locations,
                PageInfo = new PageInfo
                {
                    Items = crashes.Count(),
                    PerPage = cardsPerPage,
                    CurrentPage = pageNum
                }
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //COOKIE SECTION


        public IActionResult Admin()
        {
            var crashes = _repo.Crashes.Include("Location").ToList();
            return View(crashes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Locations = _repo.Locations.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Add(Crash c)
        {
            if (ModelState.IsValid)
            {
                var crashes = _repo.Crashes.ToList();
                var max = 0;
                foreach(Crash crash in crashes)
                {
                    if (max < crash.CrashId)
                    {
                        max = crash.CrashId;
                    }
                }
                c.CrashId = max + 1;
                _repo.Add(c);
                return View("Confirmation", c);
            }
            else
            {
                ViewBag.Locations = _repo.Locations.ToList();
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int crashid)
        {
            _repo.Update(c);
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public IActionResult Edit(Crash c)
        {

        }
        
    }
}
