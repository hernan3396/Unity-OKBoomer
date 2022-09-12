using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        EventManager.Pause += OnPause;
        EventManager.MainMenu += OnMainMenu;
    }

    private void OnPause(bool value)
    {
        if (value)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnMainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
        EventManager.MainMenu -= OnMainMenu;
    }
}
