using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YeetCarAccidents.Models
{
	public class Location
	{
        [Key]
        [Column("LOCATION_ID")]
        public int LocationID { get; set; }
        [Column("LAT_UTM_Y")]
        public Single Latitude { get; set; }
        [Column("LONG_UTM_X")]
        public Single Longitude { get; set; }
        [Column("MAIN_ROAD_NAME")]
        public string RoadName { get; set; }
        [Column("CITY")]
        public string City { get; set; }
        [Column("COUNTY_NAME")]
        public string County { get; set; }
        [Column("ROUTE")]
        public int Route { get; set; }
        [Column("MILEPOINT")]
        public Single Milepoint { get; set; }
    }
}

