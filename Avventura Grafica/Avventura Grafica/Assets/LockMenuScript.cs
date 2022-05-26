using AC;
using UnityEngine;

public class LockMenuScript : MonoBehaviour
{
    [SerializeField] string menuName;
    [SerializeField] bool lockMenu;

    private void Awake()
    {
        if (menuName != null && lockMenu == true)
        {
            LockMenu(menuName);
        } 
    }

    public void LockMenu(string menuName)
    {
        Menu myMenu = MenuManager.GetMenuWithName(menuName);
        myMenu.isLocked = true;
    }

    public void UnlockMenu(string menuName)
    {
        Menu myMenu = MenuManager.GetMenuWithName(menuName);
        myMenu.isLocked = false;
    }
}
