using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    #region Components
    private UIManager _uiManager;
    #endregion

    private string _levelName;

    private bool _isPaused = false;
    private float time;
    private string totalTime;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();

        EventManager.NextLevel += OnLevelFinished;
        EventManager.Pause += OnPause;

        _levelName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (_isPaused) return;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        time += Time.deltaTime;
        totalTime = Utils.FloatToTime(time);

        _uiManager.UpdateUIText(UIManager.Element.Timer, totalTime);
    }

    private void OnLevelFinished()
    {
        _isPaused = true;
        EventManager.OnSaveTime(_levelName, time);
    }

    private void OnPause(bool value)
    {
        _isPaused = value;
    }

    private void OnDestroy()
    {
        EventManager.NextLevel -= OnLevelFinished;
        EventManager.Pause -= OnPause;
    }
}
