using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

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

    [SerializeField] private RectTransform _levels;
    [SerializeField] private List<Button> _levelsBtn = new List<Button>();

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
        foreach (RectTransform item in _levels)
        {
            _levelsBtn.Add(item.GetComponent<Button>());
        }
    }

    private void Start()
    {
        _currentCG = (int)CanvasGroups.MainMenu;

        ManageEvents();
    }

    private void ManageEvents()
    {
        EventManager.OnFadeIn(_canvasElements[(int)_currentCG], _fadeDur);
        EventManager.GameLoaded += GameLoaded;
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

    private void GameLoaded(SaveData saveData)
    {
        ChangeTimers(saveData.TimerInfo);
        ActivateLevels(saveData.TimerInfo.Count);
        if (saveData.OnALevel) ActivateContinue();
    }

    private void ChangeTimers(List<TimerData> timerInfo)
    {
        for (int i = 0; i < timerInfo.Count; i++)
            _timers[i].text = Utils.FloatToTime(timerInfo[i].LevelTime);
    }

    private void ActivateContinue()
    {
        _continueBtn.interactable = true;
    }

    private void ActivateLevels(int maxLevel)
    {
        maxLevel += 1; // le sumamos uno porque queremos desbloquear el siguiente

        if (maxLevel >= _levelsBtn.Count)
            maxLevel = _levelsBtn.Count;

        for (int i = 0; i < maxLevel; i++)
            _levelsBtn[i].interactable = true;

        for (int i = maxLevel; i < _levelsBtn.Count; i++)
            _levelsBtn[i].interactable = false;
    }

    private void OnDestroy()
    {
        EventManager.GameLoaded -= GameLoaded;
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