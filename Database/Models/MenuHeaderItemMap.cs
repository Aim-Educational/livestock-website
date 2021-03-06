using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("menu_header_item_map")]
    public partial class MenuHeaderItemMap
    {
        [Column("menu_header_item_map_id")]
[DisplayName("Menu Header Item Map")]
        public int MenuHeaderItemMapId { get; set; }
        [Column("menu_header_id")]
[DisplayName("Menu Header")]
        public int MenuHeaderId { get; set; }
        [Column("menu_item_id")]
[DisplayName("Menu Item")]
        public int MenuItemId { get; set; }
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

        [ForeignKey("MenuHeaderId")]
        [InverseProperty("MenuHeaderItemMap")]
        public virtual MenuHeader MenuHeader { get; set; }
        [ForeignKey("MenuItemId")]
        [InverseProperty("MenuHeaderItemMap")]
        public virtual MenuItem MenuItem { get; set; }
    }
}
