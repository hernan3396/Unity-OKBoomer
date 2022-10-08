using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    #region Components
    private UIManager _uiManager;
    #endregion

    private string _levelName;

    private bool _isPaused = false;
    private float _time;
    private string totalTime;

    private float _timeCheckpoint;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();

        EventManager.NextLevel += OnLevelFinished;
        EventManager.Pause += OnPause;

        _levelName = SceneManager.GetActiveScene().name;
        EventManager.Checkpoint += SetCheckpointTime;
    }

    private void Start()
    {
        EventManager.GameStart += StartGame;

        LoadSaveData();
    }

    private void Update()
    {
        if (_isPaused) return;
        UpdateTimer();
    }

    private void LoadSaveData()
    {
        SaveData saveData = GameManager.GetInstance.GetSaveData;

        if (saveData.OnALevel)
        {
            _timeCheckpoint = saveData.CheckpointTimer;
            _time = _timeCheckpoint;
        }
    }

    private void StartGame()
    {
        // esta separado para no cargar del gamemanager cada vez que respawneas
        _time = _timeCheckpoint;
    }

    private void SetCheckpointTime()
    {
        _timeCheckpoint = _time;
        GameManager.GetInstance.SaveCheckpointTime(_timeCheckpoint, _levelName);
    }

    private void UpdateTimer()
    {
        _time += Time.deltaTime;
        totalTime = Utils.FloatToTime(_time);

        _uiManager.UpdateUIText(UIManager.Element.Timer, totalTime);
    }

    private void OnLevelFinished()
    {
        _isPaused = true;
        TimerData newTimerData = new TimerData(_levelName, _time);
        GameManager.GetInstance.OnLevelFinished(newTimerData);
    }

    private void OnPause(bool value)
    {
        _isPaused = value;
    }

    private void OnDestroy()
    {
        EventManager.NextLevel -= OnLevelFinished;
        EventManager.Pause -= OnPause;
        EventManager.GameStart -= StartGame;
        EventManager.Checkpoint -= SetCheckpointTime;
    }
}
