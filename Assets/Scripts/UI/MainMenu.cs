using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public enum CanvasGroups
    {
        MainMenu,
        SelectLevel
    }

    #region CanvasGroup
    [Header("Canvas group")]
    [SerializeField] private CanvasElement[] _canvasElements;
    private CanvasGroups _currentCG;
    [SerializeField] private Button _continueBtn;
    #endregion

    #region Animations
    [Header("Animations")]
    [SerializeField] private int _cameraSpeed;
    [SerializeField] private int _fadeDur;
    #endregion

    #region Timers
    [Header("Timers")]
    [SerializeField] private TMP_Text[] _timers;
    #endregion

    private void Awake()
    {
        EventManager.LoadTimer += ChangeTimers;
        EventManager.ActivateContinueBtn += ActivateContinue;
    }

    private void Start()
    {
        _currentCG = (int)CanvasGroups.MainMenu;

        ManageEvents();
    }

    private void ManageEvents()
    {
        EventManager.OnFadeIn(_canvasElements[(int)_currentCG], _fadeDur);
        EventManager.OnInfiniteRotate(_cameraSpeed);
        EventManager.OnMainMenu();
    }

    public void ChangeCG(int nextCG)
    {
        StartCoroutine("ChangingCG", nextCG);
    }

    private IEnumerator ChangingCG(int nextCG)
    {
        EventManager.OnFadeOut(_canvasElements[(int)_currentCG], _fadeDur);
        yield return new WaitForSeconds(_fadeDur);

        _currentCG = (CanvasGroups)nextCG;
        EventManager.OnFadeIn(_canvasElements[(int)_currentCG], _fadeDur);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ChangeTimers(float[] times)
    {
        for (int i = 0; i < times.Length; i++)
            _timers[i].text = Utils.FloatToTime(times[i]);
    }

    private void ActivateContinue()
    {
        _continueBtn.interactable = true;
    }

    private void OnDestroy()
    {
        EventManager.LoadTimer -= ChangeTimers;
        EventManager.ActivateContinueBtn -= ActivateContinue;
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