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
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Dashboard(int pageNum = 1)
        {
            const int cardsPerPage = 10;
            var crashes = await _repo.Crashes.Take(cardsPerPage * 3).ToListAsync();
            var location = await _repo.Location.Take(cardsPerPage * 3).ToListAsync(); //todo: add location filtering
            //skip based on page num with each subsequent request. Use count to calculate pages.

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
        public IActionResult CrashChange()
        {
            ViewBag.Location = _repo.Location.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult CrashChange(Crash c)
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
                _repo.AddCrash(c);
                return View("Confirmation", c);
            }
            else
            {
                ViewBag.Location = _repo.Location.ToList();
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int crashid)
        {
            ViewBag.Location = _repo.Location.ToList();
            var crash = _repo.Crashes.Single(x => x.CrashId == crashid);
            return RedirectToAction("CrashChange", crash);
        }
        [HttpPost]
        public IActionResult Edit(Crash c)
        {
            _repo.UpdateCrash(c);
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult Delete(int crashid)
        {
            var crash = _repo.Crashes.Single(x => x.CrashId == crashid);
            return View(crash);
        }

        [HttpPost]
        public IActionResult Delete(Crash crash)
        {
            var c = _repo.Crashes.Single(x => x.CrashId == crash.CrashId);
            _repo.DeleteCrash(c);
            return RedirectToAction("Admin");
        }
        
    }
}
