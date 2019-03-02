using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class VehicleTrailerMap
    {
        public VehicleTrailerMap()
        {
            VehicleLifeEvent = new HashSet<VehicleLifeEvent>();
        }

[DisplayName("Vehicle Trailer Map")]
        public int VehicleTrailerMapId { get; set; }
[DisplayName("Vehicle Main")]
        public int VehicleMainId { get; set; }
[DisplayName("Trailer")]
        public int TrailerId { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual Vehicle Trailer { get; set; }
        public virtual Vehicle VehicleMain { get; set; }
        public virtual ICollection<VehicleLifeEvent> VehicleLifeEvent { get; set; }
    }
}
