using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("vehicle_life_event_wash")]
    public partial class VehicleLifeEventWash
    {
        [Column("vehicle_life_event_wash_id")]
[DisplayName("Vehicle Life Event Wash")]
        public int VehicleLifeEventWashId { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
        [Column("user_id")]
[DisplayName("User")]
        public int UserId { get; set; }
        [Column("vehicle_life_event_id")]
[DisplayName("Vehicle Life Event")]
        public int VehicleLifeEventId { get; set; }
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
    }
}
