using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("vehicle_life_event")]
    public partial class VehicleLifeEvent
    {
        [Column("vehicle_life_event_id")]
[DisplayName("Vehicle Life Event")]
        public int VehicleLifeEventId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
        [Column("enum_vehicle_life_event_type_id")]
[DisplayName("Enum Vehicle Life Event Type")]
        public int EnumVehicleLifeEventTypeId { get; set; }
        [Column("vehicle_trailer_map_id")]
[DisplayName("Vehicle Trailer Map")]
        public int VehicleTrailerMapId { get; set; }
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

        [ForeignKey("EnumVehicleLifeEventTypeId")]
        [InverseProperty("VehicleLifeEvent")]
        public virtual EnumVehicleLifeEventType EnumVehicleLifeEventType { get; set; }
        [ForeignKey("VehicleTrailerMapId")]
        [InverseProperty("VehicleLifeEvent")]
        public virtual VehicleTrailerMap VehicleTrailerMap { get; set; }
    }
}
