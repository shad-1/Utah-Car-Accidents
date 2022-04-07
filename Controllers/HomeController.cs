using Microsoft.AspNetCore.Authorization;
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
            var crashes = await _repo.Crashes.Take(cardsPerPage * 3).Include("Location").ToListAsync();
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
        //TEST
        [Authorize]
        public IActionResult SuperSecret()
        {
            return View();
        }
        //COOKIE SECTION

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

        [HttpGet]
        public async Task<IActionResult> CrashChange()
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            return View();
        }
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

        [HttpGet]
        public async Task<IActionResult> Edit(int crashid)
        {
            ViewBag.Location = await _repo.Location.ToListAsync();
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View("CrashChange", crash);
        }
        [HttpPost]
        public IActionResult Edit(Crash c)
        {
            _repo.UpdateCrash(c);
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int crashid)
        {
            var crash = await _repo.Crashes.SingleAsync(x => x.CrashId == crashid);
            return View(crash);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Crash crash)
        {
            var c = await _repo.Crashes.SingleAsync(x => x.CrashId == crash.CrashId);
            _repo.DeleteCrash(c);
            return RedirectToAction("Admin");
        }
        
    }
}
