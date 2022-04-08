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
        #nullable enable
        [Column("LAT_UTM_Y")]
        public float? Latitude { get; set; }
        [Column("LONG_UTM_X")]
        public float? Longitude { get; set; }
        [Column("MAIN_ROAD_NAME")]
        public string? RoadName { get; set; }
        [Column("CITY")]
        public string? City { get; set; }
        [Column("COUNTY_NAME")]
        public string? County { get; set; }
        [Column("ROUTE")]
        public string? Route { get; set; }
        [Column("MILEPOINT")]
        public float? Milepoint { get; set; }
    }
}

