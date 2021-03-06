using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    [Table("menu_header")]
    public partial class MenuHeader
    {
        public MenuHeader()
        {
            InverseMenuHeaderParent = new HashSet<MenuHeader>();
            MenuHeaderItemMap = new HashSet<MenuHeaderItemMap>();
        }

        [Column("menu_header_id")]
[DisplayName("Menu Header")]
        public int MenuHeaderId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
[DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [Column("title")]
        [StringLength(50)]
[DisplayName("Title")]
        public string Title { get; set; }
        [Column("application_code")]
[DisplayName("Application Code")]
        public int ApplicationCode { get; set; }
        [Column("role_id")]
[DisplayName("Role")]
        public int RoleId { get; set; }
        [Column("menu_header_parent_id")]
[DisplayName("Menu Header Parent")]
        public int MenuHeaderParentId { get; set; }
        [Required]
        [Column("image_uri")]
        [StringLength(255)]
[DisplayName("Image Uri")]
        public string ImageUri { get; set; }
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

        [ForeignKey("MenuHeaderParentId")]
        [InverseProperty("InverseMenuHeaderParent")]
        public virtual MenuHeader MenuHeaderParent { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("MenuHeader")]
        public virtual Role Role { get; set; }
        [InverseProperty("MenuHeaderParent")]
        public virtual ICollection<MenuHeader> InverseMenuHeaderParent { get; set; }
        [InverseProperty("MenuHeader")]
        public virtual ICollection<MenuHeaderItemMap> MenuHeaderItemMap { get; set; }
    }
}
