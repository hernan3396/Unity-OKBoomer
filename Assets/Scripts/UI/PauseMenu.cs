using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _selectedBtn;

    private void Awake()
    {
        _selectedBtn.Select();
    }

    public void Resume()
    {
        // EventManager.OnResumeMenu();
    }

    public void MainMenu(string value)
    {
        // EventManager.OnGoToNextLevel(value);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
