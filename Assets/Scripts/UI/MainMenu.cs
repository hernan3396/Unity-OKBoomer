using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    #region CanvasGroup
    [Header("Canvas Group")]
    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _levelsPanel;
    private CanvasGroup _mainGroup;
    private CanvasGroup _levelsGroup;

    [SerializeField] private RectTransform _levels;
    [SerializeField] private List<Button> _levelsBtn = new List<Button>();

    [SerializeField] private Button _continueBtn;
    #endregion

    #region Timers
    [Header("Timers")]
    [SerializeField] private TMP_Text[] _timers;
    #endregion

    #region StartMenu
    [Header("StartMenu")]
    [SerializeField] private Button _startButton;
    private bool _startMenuVisible = false;
    private bool _isAnimating = false;
    private Vector2 _startMenuInitPos;
    #endregion

    #region LevelsMenu
    [SerializeField] private Button _selectLevel;
    [SerializeField] private Button _backBtn;
    private bool _levelMenuVisible = false;
    private Vector2 _levelMenuInitPos;
    #endregion

    private void Awake()
    {
        foreach (RectTransform item in _levels)
        {
            Button itemBtn = item.GetComponent<Button>();
            _levelsBtn.Add(itemBtn);
            itemBtn.onClick.AddListener(() => OnLevelItemClick(itemBtn.name));
        }
    }

    private void Start()
    {
        ManageEvents();
        InitializeUI();
    }

    private void ManageEvents()
    {
        EventManager.GameLoaded += GameLoaded;
        EventManager.OnMainMenu();
    }

    #region Menu
    private void OnStartButtonClick()
    {
        if (_isAnimating) return;

        _startMenuVisible = !_startMenuVisible;

        if (_startMenuVisible) ShowStartMenu();
        else
        {
            HideStartMenu();
            if (_levelMenuVisible)
            {
                _levelMenuVisible = false;
                HideLevelsMenu();
            }
        }
    }

    private void ShowStartMenu()
    {
        _isAnimating = true;
        _mainMenu.gameObject.SetActive(true);
        _mainGroup.DOFade(1, 0.5f);

        _mainMenu.DOAnchorPos(_startMenuInitPos, 0.5f)
        .OnComplete(() => _isAnimating = false);
    }

    private void HideStartMenu()
    {
        _isAnimating = true;
        _mainGroup.DOFade(0, 0.5f);
        Vector2 resultPos = _mainMenu.anchoredPosition - new Vector2(0, 100);

        _mainMenu.DOAnchorPos(resultPos, 0.5f).OnComplete(() =>
        {
            _mainMenu.gameObject.SetActive(false);
            _isAnimating = false;
        });
    }
    #endregion

    #region LevelSelect
    private void OnSelectLevelClick()
    {
        if (_isAnimating) return;

        _levelMenuVisible = !_levelMenuVisible;

        if (_levelMenuVisible) ShowLevelsMenu();
        else HideLevelsMenu();
    }

    private void ShowLevelsMenu()
    {
        _isAnimating = true;
        _levelsPanel.gameObject.SetActive(true);
        _levelsGroup.DOFade(1, 0.5f);

        _levelsPanel.DOAnchorPos(_levelMenuInitPos, 0.5f)
        .OnComplete(() => _isAnimating = false);
    }

    private void HideLevelsMenu()
    {
        _isAnimating = true;
        _levelsGroup.DOFade(0, 0.5f);
        Vector2 resultPos = _levelsPanel.anchoredPosition - new Vector2(30, 0);

        _levelsPanel.DOAnchorPos(resultPos, 0.5f)
        .OnComplete(() =>
        {
            _levelsPanel.gameObject.SetActive(false);
            _isAnimating = false;
        });
    }
    #endregion

    private void InitializeUI()
    {
        _mainMenu.gameObject.SetActive(false);
        _levelsPanel.gameObject.SetActive(false);

        _mainGroup = _mainMenu.GetComponent<CanvasGroup>();
        _levelsGroup = _levelsPanel.GetComponent<CanvasGroup>();

        _startMenuInitPos = _mainMenu.anchoredPosition;
        _mainMenu.anchoredPosition -= new Vector2(0, 100);

        _levelMenuInitPos = _levelsPanel.anchoredPosition;
        _levelsPanel.anchoredPosition -= new Vector2(30, 0);

        _startButton.onClick.AddListener(OnStartButtonClick);
        _selectLevel.onClick.AddListener(OnSelectLevelClick);
        _backBtn.onClick.AddListener(OnSelectLevelClick);
    }

    private void OnLevelItemClick(string buttonName)
    {
        EventManager.OnChangeLevel(buttonName);
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