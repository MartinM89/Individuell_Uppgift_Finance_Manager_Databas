public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public byte[]? PasswordHash { get; private set; }
    public byte[]? PasswordSalt { get; private set; }

    public User(string username, byte[] passwordHash, byte[] passwordSalt)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }
}
