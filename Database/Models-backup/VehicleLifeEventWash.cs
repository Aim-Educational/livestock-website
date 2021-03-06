using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class VehicleLifeEventWash
    {
[DisplayName("Vehicle Life Event Wash")]
        public int VehicleLifeEventWashId { get; set; }
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
[DisplayName("User")]
        public int UserId { get; set; }
[DisplayName("Vehicle Life Event")]
        public int VehicleLifeEventId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
