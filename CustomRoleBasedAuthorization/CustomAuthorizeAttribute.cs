using CustomRoleBasedAuthorization.Database.Data;
using CustomRoleBasedAuthorization.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomRoleBasedAuthorization;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredRoles;

    public CustomAuthorizeAttribute(params string[] requiredRoles)
    {
        _requiredRoles = requiredRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
        {
            context.Result = new UnauthorizedResult(); // 401 Unauthorized
            return;
        }

        try
        {
            var (userId, sessionId, roles) = TokenService.DecryptToken(token!);

            using (var db = new ApplicationDbContext())
            {
                var session = db.TblUserLogins
                    .FirstOrDefault(ul => ul.UserId == userId && ul.SessionId == sessionId && ul.SessionExpiredDate > DateTime.UtcNow && ul.LogoutDate == null);

                if (session == null)
                {
                    context.Result = new UnauthorizedResult(); // 401 Unauthorized
                    return;
                }
            }

            if (_requiredRoles.Length > 0 && !_requiredRoles.Any(role => roles.Contains(role)))
            {
                // Return 403 Forbidden if the user doesn't have the required roles
                context.Result = new StatusCodeResult(403); // 403 Forbidden
                return;
            }

            // Store user information in HttpContext for later use
            context.HttpContext.Items["UserId"] = userId;
            context.HttpContext.Items["Roles"] = roles;
        }
        catch
        {
            context.Result = new UnauthorizedResult(); // 401 Unauthorized
        }
    }
}