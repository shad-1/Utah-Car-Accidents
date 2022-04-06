using System;
using System.Collections.Generic;

namespace YeetCarAccidents.Models.ViewModels
{
	public class DashboardViewModel
	{
		public List<Crash> Crashes { get; set; }
		public List<Location> Locations { get; set; }
		public PageInfo PageInfo { get; set; }
	}
}

