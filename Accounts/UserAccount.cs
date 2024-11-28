public class UserAccount : IAccount
{
    readonly CreateAccountCommand create = new();
    readonly LoginCommand login = new();

    public void Create()
    {
        create.RunCommand();
    }

    public Guid GetUserId(Guid id)
    {
        return id;
    }

    public void Login()
    {
        login.RunCommand();
    }
}
