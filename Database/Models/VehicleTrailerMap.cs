using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("vehicle_trailer_map")]
    public partial class VehicleTrailerMap
    {
        public VehicleTrailerMap()
        {
            VehicleLifeEvent = new HashSet<VehicleLifeEvent>();
        }

        [Column("vehicle_trailer_map_id")]
[DisplayName("Vehicle Trailer Map")]
        public int VehicleTrailerMapId { get; set; }
        [Column("vehicle_main_id")]
[DisplayName("Vehicle Main")]
        public int VehicleMainId { get; set; }
        [Column("trailer_id")]
[DisplayName("Trailer")]
        public int TrailerId { get; set; }
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

        [ForeignKey("TrailerId")]
        [InverseProperty("VehicleTrailerMapTrailer")]
        public virtual Vehicle Trailer { get; set; }
        [ForeignKey("VehicleMainId")]
        [InverseProperty("VehicleTrailerMapVehicleMain")]
        public virtual Vehicle VehicleMain { get; set; }
        [InverseProperty("VehicleTrailerMap")]
        public virtual ICollection<VehicleLifeEvent> VehicleLifeEvent { get; set; }
    }
}
