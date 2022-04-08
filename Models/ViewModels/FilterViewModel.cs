using System;
using System.Collections.Generic;
using System.Linq;

namespace YeetCarAccidents.Models.ViewModels
{
	public class FilterViewModel
	{
		public IQueryable<string> Counties { get; set; }
		public IQueryable<string> Cities { get; set; }
		public IQueryable<string> DaysOfWeek { get; set; }
		public List<int> DaysOfMonth { get; set; }
		public List<int> Months { get; set; }
		public List<int> Years { get; set; }
	}
}

