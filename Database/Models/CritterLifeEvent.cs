using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter_life_event")]
    public partial class CritterLifeEvent
    {
        [Column("critter_life_event_id")]
[DisplayName("Critter Life Event")]
        public int CritterLifeEventId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
        [Column("enum_critter_life_event_type_id")]
[DisplayName("Enum Critter Life Event Type")]
        public int EnumCritterLifeEventTypeId { get; set; }
        [Column("enum_critter_life_event_data_id")]
[DisplayName("Enum Critter Life Event Data")]
        public int EnumCritterLifeEventDataId { get; set; }
        [Column("critter_id")]
[DisplayName("Critter")]
        public int CritterId { get; set; }
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

        [ForeignKey("CritterId")]
        [InverseProperty("CritterLifeEvent")]
        public virtual Critter Critter { get; set; }
        [ForeignKey("EnumCritterLifeEventTypeId")]
        [InverseProperty("CritterLifeEvent")]
        public virtual EnumCritterLifeEventType EnumCritterLifeEventType { get; set; }
    }
}
