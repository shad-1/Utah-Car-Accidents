using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using YeetCarAccidents.Data;

namespace YeetCarAccidents.Components
{
	public class FilterViewComponent : ViewComponent
	{
        private ICrashRepository _repo { get; set; }

    public FilterViewComponent(ICrashRepository repo)
    {
        _repo = repo;
    }

    public IViewComponentResult Invoke()
		{
			//Get all the distinct counties from all crashes
			var counties = _repo.Location
                .Select(l => l.County)
                .Distinct()
                .OrderBy(l => l);

            return View(counties);
		}
	}
}

