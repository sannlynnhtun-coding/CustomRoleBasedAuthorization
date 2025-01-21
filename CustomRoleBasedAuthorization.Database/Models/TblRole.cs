using System;
using System.Collections.Generic;

namespace CustomRoleBasedAuthorization.Database.Models;

public partial class TblRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
