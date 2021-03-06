using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("enum_vehicle_life_event_type")]
    public partial class EnumVehicleLifeEventType
    {
        public EnumVehicleLifeEventType()
        {
            VehicleLifeEvent = new HashSet<VehicleLifeEvent>();
        }

        [Column("enum_vehicle_life_event_type_id")]
[DisplayName("Enum Vehicle Life Event Type")]
        public int EnumVehicleLifeEventTypeId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
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

        [InverseProperty("EnumVehicleLifeEventType")]
        public virtual ICollection<VehicleLifeEvent> VehicleLifeEvent { get; set; }
    }
}
