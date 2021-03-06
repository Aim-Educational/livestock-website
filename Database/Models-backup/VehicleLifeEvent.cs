using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class VehicleLifeEvent
    {
[DisplayName("Vehicle Life Event")]
        public int VehicleLifeEventId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
[DisplayName("Enum Vehicle Life Event Type")]
        public int EnumVehicleLifeEventTypeId { get; set; }
[DisplayName("Vehicle Trailer Map")]
        public int VehicleTrailerMapId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }
    }
}
