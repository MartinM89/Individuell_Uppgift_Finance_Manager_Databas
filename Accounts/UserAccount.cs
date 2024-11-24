public class UserAccount : IAccount
{
    CreateAccountCommand create = new CreateAccountCommand();
    LoginCommand login = new LoginCommand();

    public void Create()
    {
        create.RunCommand();
    }

    public void Login()
    {
        login.RunCommand();
    }
}
