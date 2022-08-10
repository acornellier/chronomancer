using UnityEngine;
using Zenject;

public class Menu : MonoBehaviour
{
    [Inject] protected MenuManager menuManager;

    public void GoBackOrResume()
    {
        menuManager.GoBackOrResume();
    }

    public void GoBack()
    {
        menuManager.GoBack();
    }

    public void OpenMenu(GameObject menu)
    {
        menuManager.OpenMenu(menu);
    }
}
