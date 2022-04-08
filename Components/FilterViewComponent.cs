using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YeetCarAccidents.Data;
using YeetCarAccidents.Models.ViewModels;

namespace YeetCarAccidents.Components
{
	public class FilterViewComponent : ViewComponent
	{
        private ICrashRepository _repo { get; set; }

    public FilterViewComponent(ICrashRepository repo)
    {
        _repo = repo;
    }

    public IViewComponentResult Invoke(FilterInfo filter)
		{
            var locations = _repo.Location.AsNoTracking();

            // Get all the distinct counties from all crashes
            var counties = locations
                .Select(l => l.County)
                .Distinct()
                .OrderBy(l => l);

            // Cities where crashes occurred 
            var cities = locations
                .Select(l => l.City)
                .Distinct()
                .OrderBy(l => l);

            //Data to setup remaining filtering components
            var daysOfWeek = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            var daysOfMonth = new int[31];
            var months = new int[12]; //C# dates are compared as integers

            for (int i = 1; i <= 31; i++)
            {
                daysOfMonth[i - 1] = i;
                if (i <= 12)
                    months[i - 1] = i;
            }

            var years = new int[] { 2016, 2017, 2019, 2020 };

            var vm = new FilterViewModel
            {
                Counties = counties,
                Cities = cities,
                DaysOfWeek = daysOfWeek.AsQueryable(),
                DaysOfMonth = daysOfMonth.ToList(),
                Months = months.ToList(),
                Years = years.ToList()
            };

            //use it in the viewbag so we can bind the FilterInfo model to the form inputs
            ViewBag.FilterViewModel = vm;

            return View(filter);
		}
	}
}

