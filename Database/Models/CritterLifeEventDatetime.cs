using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("critter_life_event_datetime")]
    public partial class CritterLifeEventDatetime
    {
        [Column("critter_life_event_give_birth_id")]
[DisplayName("Critter Life Event Give Birth")]
        public int CritterLifeEventGiveBirthId { get; set; }
        [Column("date_time", TypeName = "datetime")]
[DisplayName("Date Time")]
        public DateTime DateTime { get; set; }
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
