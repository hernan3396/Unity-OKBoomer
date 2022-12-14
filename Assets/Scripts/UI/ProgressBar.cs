using UnityEngine;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private CanvasGroup _progressCG;
    [SerializeField] private GameObject _progressGO;

    [Header("Duration")]
    [SerializeField] private float _tweenDur = 0.5f;
    [SerializeField] private int _fadeDur = 2;

    private int _maxWidth;
    private int _maxvalue;
    private Tween _currentTween;

    private void Awake()
    {
        _progressGO.SetActive(false);
        _progressCG.alpha = 0;

        _maxWidth = (int)_progressBar.sizeDelta.x;

        EventManager.StartProgressBar += StartProgressBar;
        EventManager.DeactivateProgressBar += Deactivate;
        EventManager.ForceDeactivate += ForceDeactivate;
        _progressBar.DOSizeDelta(new Vector2(0, _progressBar.sizeDelta.y), 0); // pone el valor en 0
    }

    public void StartProgressBar(int maxValue)
    {
        EventManager.UpdateProgressBar += UpdateProgressBar;
        _progressCG.alpha = 0;

        _maxvalue = maxValue;
        UpdateProgressBar(0);

        _progressGO.SetActive(true);
        _progressCG.DOFade(1, _fadeDur);
    }

    public void UpdateProgressBar(int actualValue)
    {
        if (_currentTween != null)
            _currentTween.Kill();

        float relativeValue = 1 - (float)actualValue / _maxvalue;
        float relativeSize = relativeValue * _maxWidth;

        _currentTween = _progressBar.DOSizeDelta(new Vector2(relativeSize, _progressBar.sizeDelta.y), _tweenDur);

        if (actualValue >= _maxvalue)
            _currentTween = _progressCG.DOFade(0, _fadeDur).OnComplete(() => Deactivate());
    }

    private void Deactivate()
    {
        EventManager.UpdateProgressBar -= UpdateProgressBar;
        _progressGO.SetActive(false);

        if (_currentTween != null)
            _currentTween.Kill();
    }

    private void ForceDeactivate()
    {
        EventManager.UpdateProgressBar -= UpdateProgressBar;
    }

    private void OnDestroy()
    {
        EventManager.StartProgressBar -= StartProgressBar;
        EventManager.UpdateProgressBar -= UpdateProgressBar;
        EventManager.DeactivateProgressBar -= Deactivate;
        EventManager.ForceDeactivate -= ForceDeactivate;

        if (_currentTween != null)
            _currentTween.Kill();
    }
}
