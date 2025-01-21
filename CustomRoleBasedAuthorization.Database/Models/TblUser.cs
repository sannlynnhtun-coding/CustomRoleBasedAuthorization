using System;
using System.Collections.Generic;

namespace CustomRoleBasedAuthorization.Database.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();

    public virtual ICollection<TblUserLogin> TblUserLogins { get; set; } = new List<TblUserLogin>();
}
