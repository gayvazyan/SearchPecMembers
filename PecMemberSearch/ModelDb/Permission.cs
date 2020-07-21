using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PecMemberSearch.ModelDb
{
    public partial class Permission
    {
        [Key]
        [Column("PermissionID")]
        public int PermissionId { get; set; }
        public string SystemRole { get; set; }
        [Column("MenuItemID")]
        public int? MenuItemId { get; set; }

        [ForeignKey(nameof(MenuItemId))]
        [InverseProperty("Permission")]
        public virtual MenuItem MenuItem { get; set; }
    }
}
