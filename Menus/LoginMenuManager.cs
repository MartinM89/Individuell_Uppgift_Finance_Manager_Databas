using Individuell_Uppgift.Menus;

public class LoginMenuManager : IMenuManager
{
    private Menu menu = new EmptyMenu();

    public Menu GetMenu()
    {
        return menu;
    }

    public void SetMenu(Menu menu)
    {
        this.menu = menu;
        menu.Display();
    }

    public void ReturnToSameMenu()
    {
        menu.Display();
    }
}
