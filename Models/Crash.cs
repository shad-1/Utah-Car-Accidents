using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YeetCarAccidents.Models
{
    public class Crash
    {
        [Key]
        [Column("CRASH_ID")]
        public int CrashId { get; set; }

        [Column("CRASH_DATETIME")]
        public DateTime DateTime { get; set; }

        [Column("LOCATION_ID")]
        public int LocationID { get; set; }
        public Location Location { get; set; }

        [Column("CRASH_SEVERITY_ID")]
        public int Severity { get; set; }

        [Column("WORK_ZONE_RELATED")]
        public bool WorkZone { get; set; }
        [Column("PEDESTRIAN_INVOLVED")]
        public bool Pedestrian { get; set; }
        [Column("BICYCLIST_INVOLVED")]
        public bool Bicyclist { get; set; }
        [Column("MOTORCYCLE_INVOLVED")]
        public bool Motorcycle { get; set; }
        [Column("IMPROPER_RESTRAINT")]
        public bool ImproperRestraint { get; set; }
        [Column("UNRESTRAINED")]
        public bool Unrestrained { get; set; }
        [Column("DUI")]
        public bool Dui { get; set; }
        [Column("INTERSECTION_RELATED")]
        public bool Intersection { get; set; }
        [Column("WILD_ANIMAL_RELATED")]
        public bool WildAnimal { get; set; }
        [Column("DOMESTIC_ANIMAL_RELATED")]
        public bool DomesticAnimal { get; set; }
        [Column("OVERTURN_ROLLOVER")]
        public bool Rollover { get; set; }
        [Column("COMMERCIAL_MOTOR_VEH_INVOLVED")]
        public bool Commercial { get; set; }
        [Column("TEENAGE_DRIVER_INVOLVED")]
        public bool Teenage { get; set; }
        [Column("OLDER_DRIVER_INVOLVED")]
        public bool Older { get; set; }
        [Column("NIGHT_DARK_CONDITION")]
        public bool Night { get; set; }
        [Column("SINGLE_VEHICLE")]
        public bool SingleVehicle { get; set; }
        [Column("DISTRACTED_DRIVING")]
        public bool Distracted { get; set; }
        [Column("DROWSY_DRIVING")]
        public bool Drowsy { get; set; }
        [Column("ROADWAY_DEPARTURE")]
        public bool RoadwayDeparture { get; set; }
    }
}

