using System.Security.Cryptography;
using System.Text;

public class PasswordHasher
{
    // Method to create a password hash and salt
    public static void CreatePasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt
    )
    {
        using (var hmac = new HMACSHA512())
        {
            // Generate a unique salt
            passwordSalt = hmac.Key;

            // Compute the hash of the password
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    // Method to verify a password against a hash and salt
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt)) // Use the original salt
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Compare the hash byte by byte
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                    return false;
            }

            return true;
        }
    }
}
