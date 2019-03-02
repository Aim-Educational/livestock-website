using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            VehicleTrailerMapTrailer = new HashSet<VehicleTrailerMap>();
            VehicleTrailerMapVehicleMain = new HashSet<VehicleTrailerMap>();
        }

[DisplayName("Vehicle")]
        public int VehicleId { get; set; }
[DisplayName("Name")]
        public string Name { get; set; }
[DisplayName("Registration Number")]
        public string RegistrationNumber { get; set; }
[DisplayName("Weight Kg")]
        public double WeightKg { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<VehicleTrailerMap> VehicleTrailerMapTrailer { get; set; }
        public virtual ICollection<VehicleTrailerMap> VehicleTrailerMapVehicleMain { get; set; }
    }
}
