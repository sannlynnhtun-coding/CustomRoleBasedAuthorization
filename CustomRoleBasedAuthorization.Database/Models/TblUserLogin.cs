using System;
using System.Collections.Generic;

namespace CustomRoleBasedAuthorization.Database.Models;

public partial class TblUserLogin
{
    public int UserLoginId { get; set; }

    public int UserId { get; set; }

    public string SessionId { get; set; } = null!;

    public DateTime SessionExpiredDate { get; set; }

    public DateTime? LogoutDate { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
