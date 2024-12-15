using System.Security.Cryptography;
using System.Text;

namespace Individuell_Uppgift.Utilities;

public class PasswordHasher
{
    public static (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        using HMACSHA512 hmac = new();
        byte[] passwordSalt = hmac.Key;

        byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return (passwordHash, passwordSalt);
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);
        byte[]? computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
                return false;
        }

        return true;
    }
}
