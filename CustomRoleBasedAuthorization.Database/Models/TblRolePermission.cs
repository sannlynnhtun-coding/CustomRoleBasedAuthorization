using System;
using System.Collections.Generic;

namespace CustomRoleBasedAuthorization.Database.Models;

public partial class TblRolePermission
{
    public int RolePermissionId { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public virtual TblRole Role { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
