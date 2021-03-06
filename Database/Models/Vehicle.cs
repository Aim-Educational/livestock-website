using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("vehicle")]
    public partial class Vehicle
    {
        public Vehicle()
        {
            VehicleTrailerMapTrailer = new HashSet<VehicleTrailerMap>();
            VehicleTrailerMapVehicleMain = new HashSet<VehicleTrailerMap>();
        }

        [Column("vehicle_id")]
[DisplayName("Vehicle")]
        public int VehicleId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [Column("registration_number")]
        [StringLength(50)]
[DisplayName("Registration Number")]
        public string RegistrationNumber { get; set; }
        [Column("weight_kg")]
[DisplayName("Weight Kg")]
        public double WeightKg { get; set; }
        [Required]
        [Column("comment")]
        [StringLength(50)]
[DisplayName("Comment")]
        public string Comment { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
        [Column("version_number")]
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        [InverseProperty("Trailer")]
        public virtual ICollection<VehicleTrailerMap> VehicleTrailerMapTrailer { get; set; }
        [InverseProperty("VehicleMain")]
        public virtual ICollection<VehicleTrailerMap> VehicleTrailerMapVehicleMain { get; set; }
    }
}
