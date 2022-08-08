using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public enum CanvasGroups
    {
        MainMenu,
        SelectLevel
    }

    #region FirstSelected
    [Header("First selected")]
    [SerializeField] private Button _firstSelected;
    #endregion

    #region CanvasGroup
    [Header("Canvas group")]
    [SerializeField] private CanvasElement[] _canvasElements;
    private CanvasGroups _currentCG;
    #endregion

    private void Start()
    {
        EventManager.OnMenuFadeIn(_canvasElements[(int)CanvasGroups.MainMenu], 1);
    }

    public void ChangeCG(int nextCG)
    {
        EventManager.OnMenuFadeOut(_canvasElements[(int)_currentCG], _canvasElements[nextCG], 1);
        _currentCG = (CanvasGroups)nextCG;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[Serializable]
public class CanvasElement
{
    public Button Button;
    public CanvasGroup CanvasGroup;

    public void SelectButton()
    {
        Button.Select();
    }

    public void ActivateCG()
    {
        CanvasGroup.gameObject.SetActive(true);
        SelectButton();
    }

    public void DeactivateCG()
    {
        CanvasGroup.gameObject.SetActive(false);
    }
}