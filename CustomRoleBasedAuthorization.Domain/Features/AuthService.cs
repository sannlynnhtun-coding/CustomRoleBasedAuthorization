using System.Text;
using CustomRoleBasedAuthorization.Database.Data;
using CustomRoleBasedAuthorization.Database.Models;
using Effortless.Net.Encryption;

namespace CustomRoleBasedAuthorization.Domain.Features;

public class AuthService
{
    private readonly ApplicationDbContext _db;

    public AuthService(ApplicationDbContext db)
    {
        _db = db;
    }

    public string Login(string username, string password)
    {
        var user = _db.TblUsers.FirstOrDefault(u => u.Username == username);

        if (user == null || !Hash.Verify(HashType.SHA256, password, "YourSalt", false, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        string sessionId = Ulid.NewUlid().ToString();
        var roles = _db.TblRolePermissions
            .Where(rp => rp.UserId == user.UserId)
            .Select(rp => rp.Role.RoleName)
            .ToList();

        _db.TblUserLogins.Add(new TblUserLogin
        {
            UserId = user.UserId,
            SessionId = sessionId,
            SessionExpiredDate = DateTime.UtcNow.AddHours(1), // 1-hour session
            LogoutDate = null
        });

        _db.SaveChanges();

        return TokenService.GenerateToken(user.UserId, sessionId, roles);
    }
}