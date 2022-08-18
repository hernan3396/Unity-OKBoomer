using UnityEngine;
using UnityEngine.Events;

public class UtilTimer : MonoBehaviour, IPauseable
{

    private float _duration;
    private float _timer;
    private bool _isPaused;
    [SerializeField] private bool _isStarted = false;

    public UnityEvent OnTimerFinished;

    public delegate void OnTimerCompleted();
    public event OnTimerCompleted onTimerCompleted;

    private void Awake()
    {
        EventManager.Pause += OnPause;
    }

    private void Update()
    {
        if (!_isStarted || _isPaused) return;

        _timer += Time.deltaTime;

        if (_timer >= _duration)
        {
            onTimerCompleted?.Invoke();
            OnTimerFinished?.Invoke();
            _timer = 0;
            _isStarted = false;
        }
    }

    public void StartTimer(float value)
    {
        SetDuration(value);
        _isStarted = true;
    }

    public void SetDuration(float value)
    {
        _duration = value;
    }

    public void OnPause(bool value)
    {
        _isPaused = value;
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }

    public bool IsStarted
    {
        get { return _isStarted; }
        set { _isStarted = value; }
    }
}
