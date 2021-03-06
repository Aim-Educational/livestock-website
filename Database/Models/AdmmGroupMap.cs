using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("admm_group_map")]
    public partial class AdmmGroupMap
    {
        [Column("admm_group_map_id")]
[DisplayName("Admm Group Map")]
        public int AdmmGroupMapId { get; set; }
        [Column("group_entity_user_id")]
[DisplayName("Group Entity User")]
        public int GroupEntityUserId { get; set; }
        [Column("group_entity_data_id")]
[DisplayName("Group Entity Data")]
        public int GroupEntityDataId { get; set; }
        [Column("group_entity_data_type")]
[DisplayName("Group Entity Data Type")]
        public int GroupEntityDataType { get; set; }
        [Required]
        [Column("timestamp")]
[DisplayName("Timestamp")]
        public byte[] Timestamp { get; set; }
    }
}
