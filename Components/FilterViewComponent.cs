using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace YeetCarAccidents.Components
{
	public class FilterViewComponent : ViewComponent
	{
		//private  _repo { get; set; }

		//public FilterViewComponent(repo)
		//{
		//	_repo = repo;
		//}

		public IViewComponentResult Invoke()
		{
			//Get all the distinct counties from all crashes
			var counties = ""; //_repo.Crashes
				//.Select(x => x.County)
				//.Distinct()
				//.OrderBy(x => x);

			return View(counties);
		}
	}
}

