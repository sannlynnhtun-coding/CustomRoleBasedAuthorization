using System.Data;
using Effortless.Net.Encryption;
using Newtonsoft.Json;

namespace CustomRoleBasedAuthorization.Domain;

public static class TokenService
{
    private static byte[] key = Convert.FromBase64String("WmPQUr3jhiIv9em4r7NRGF9rzsLnPqrFl6B7vDqwNb0=");
    private static byte[] iv = Convert.FromBase64String("yzs39jDC0/cvRRaIJCfjyA==");

    public static string GenerateToken(int userId, string sessionId, List<string> roles)
    {
        var tokenData = new
        {
            UserId = userId,
            SessionId = sessionId,
            Roles = roles
        };

        string jsonToken = JsonConvert.SerializeObject(tokenData);
        var encrypted = Strings.Encrypt(jsonToken, key, iv);
        return encrypted;
    }

    public static (int UserId, string SessionId, List<string> Roles) DecryptToken(string encrypted)
    {
        var decrypted = Strings.Decrypt(encrypted, key, iv);
        var result =  JsonConvert.DeserializeObject<UserTokenModel>(decrypted);
        return (result!.UserId, result.SessionId, result.Roles);
    }

    private class UserTokenModel
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public List<string> Roles { get; set; }
    }
}