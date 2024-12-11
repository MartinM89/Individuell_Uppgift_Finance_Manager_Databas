using Individuell_Uppgift.Menus;

public interface IMenuManager
{
    void SetMenu(Menu menu);
    Menu GetMenu();
    void ReturnToSameMenu();
}
