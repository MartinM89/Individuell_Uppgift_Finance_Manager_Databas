public class UserAccount : IAccount
{
    CreateAccountCommand create = new CreateAccountCommand();
    LoginCommand login = new LoginCommand();

    public void Create()
    {
        create.RunCommand();
    }

    public int GetUserId(int id)
    {
        return id;
    }

    public void Login()
    {
        login.RunCommand();
    }
}
