using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database.Models
{
    public partial class EnumCritterLifeEventType
    {
        public EnumCritterLifeEventType()
        {
            CritterLifeEvent = new HashSet<CritterLifeEvent>();
        }

[DisplayName("Enum Critter Life Event Type")]
        public int EnumCritterLifeEventTypeId { get; set; }
[DisplayName("Description")]
        public string Description { get; set; }
[DisplayName("Comment")]
        public string Comment { get; set; }
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
[DisplayName("Version Number")]
        public int VersionNumber { get; set; }

        public virtual ICollection<CritterLifeEvent> CritterLifeEvent { get; set; }
    }
}
