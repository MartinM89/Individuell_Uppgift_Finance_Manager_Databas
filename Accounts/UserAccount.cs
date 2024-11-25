public class UserAccount : IAccount
{
    CreateAccountCommand create = new CreateAccountCommand();
    LoginCommand login = new LoginCommand();

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
