using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Components
    private UIManager _uiManager;
    #endregion

    private bool _isPaused = false;
    private float time;
    private float msec;
    private float sec;
    private float min;
    private string totalTime;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();

        EventManager.NextLevel += OnLevelFinished;
    }

    private void Update()
    {
        if (_isPaused) return;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        time += Time.deltaTime;
        msec = (int)((time - (int)time) * 100);
        sec = (int)(time % 60);
        min = (int)(time / 60 % 60);
        totalTime = string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);

        _uiManager.UpdateUIText(UIManager.Element.Timer, totalTime);
    }

    private void OnLevelFinished()
    {
        _isPaused = true;
        EventManager.OnSaveTime(totalTime, time);
    }
}
