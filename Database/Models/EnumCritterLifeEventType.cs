using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("enum_critter_life_event_type")]
    public partial class EnumCritterLifeEventType
    {
        public EnumCritterLifeEventType()
        {
            CritterLifeEvent = new HashSet<CritterLifeEvent>();
        }

        [Column("enum_critter_life_event_type_id")]
[DisplayName("Enum Critter Life Event Type")]
        public int EnumCritterLifeEventTypeId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
[DisplayName("Description")]
        public string Description { get; set; }
        [Column("enum_critter_life_event_category_id")]
[DisplayName("Enum Critter Life Event Category")]
        public int EnumCritterLifeEventCategoryId { get; set; }
        [Required]
        [Column("data_type")]
        [StringLength(50)]
[DisplayName("Data Type")]
        public string DataType { get; set; }
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
        [Column("allow_multiple")]
[DisplayName("Allow Multiple")]
        public bool AllowMultiple { get; set; }
        [Column("flag_cant_reproduce")]
[DisplayName("Flag Cant Reproduce")]
        public bool FlagCantReproduce { get; set; }
        [Column("flag_end_of_system")]
[DisplayName("Flag End Of System")]
        public bool FlagEndOfSystem { get; set; }
        [Column("flag_illness")]
[DisplayName("Flag Illness")]
        public bool FlagIllness { get; set; }

        [InverseProperty("EnumCritterLifeEventType")]
        public virtual ICollection<CritterLifeEvent> CritterLifeEvent { get; set; }
    }
}
