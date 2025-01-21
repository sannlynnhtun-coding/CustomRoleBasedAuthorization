using CustomRoleBasedAuthorization.Database.Data;
using CustomRoleBasedAuthorization.Database.Models;
using Effortless.Net.Encryption;

namespace CustomRoleBasedAuthorization.Domain.Features;

public class UserService
{
    private readonly ApplicationDbContext _db;

    public UserService(ApplicationDbContext db)
    {
        _db = db;
    }

    public void RegisterUser(string username, string password, string email, List<string> roles)
    {
        string hashedPassword = Hash.Create(HashType.SHA256, password, "YourSalt", false);

        var user = new TblUser
        {
            Username = username,
            Password = hashedPassword,
            Email = email
        };

        _db.TblUsers.Add(user);
        _db.SaveChanges();

        foreach (var roleName in roles)
        {
            var role = _db.TblRoles.FirstOrDefault(r => r.RoleName == roleName);
            if (role != null)
            {
                _db.TblRolePermissions.Add(new TblRolePermission
                {
                    RoleId = role.RoleId,
                    UserId = user.UserId
                });
            }
        }

        _db.SaveChanges();
    }
}