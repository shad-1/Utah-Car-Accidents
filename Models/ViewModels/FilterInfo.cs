using System;
using System.Collections.Generic;

namespace YeetCarAccidents.Models.ViewModels
{
	public class FilterInfo
	{
        #nullable enable
        public string? County { get; set; }
        public string? City { get; set; }
        public string? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
        public string? Month { get; set; }
        public int? Year { get; set; }
        public bool? WorkZone { get; set; }
        public bool? Pedestrian { get; set; }
        public bool? Bicyclist { get; set; }
        public bool? Motorcycle { get; set; }
        public bool? ImproperRestraint { get; set; }
        public bool? Unrestrained { get; set; }
        public bool? Dui { get; set; }
        public bool? Intersection { get; set; }
        public bool? WildAnimal { get; set; }
        public bool? DomesticAnimal { get; set; }
        public bool? Rollover { get; set; }
        public bool? Commercial { get; set; }
        public bool? Teenage { get; set; }
        public bool? Older { get; set; }
        public bool? Night { get; set; }
        public bool? SingleVehicle { get; set; }
        public bool? Distracted { get; set; }
        public bool? Drowsy { get; set; }
        public bool? RoadwayDeparture { get; set; }
    }
}
