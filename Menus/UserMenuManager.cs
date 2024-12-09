using Individuell_Uppgift.Menus;

public class UserMenuManager : IMenuManager
{
    private Menu menu = new EmptyMenu();

    public Menu GetMenu()
    {
        return menu;
    }

    public void SetMenu(Menu menu)
    {
        this.menu = menu;
        this.menu.Display();
    }

    public void ReturnToSameMenu()
    {
        this.menu.Display();
    }
}
