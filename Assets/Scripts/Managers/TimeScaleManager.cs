using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TimeScaleManager : MonoBehaviour
{
    [SerializeField] private float _bulletTimeTime = 0.2f;
    [SerializeField] private float _freezeFrameTime = 0.1f;

    private Tween _currentTween;

    private bool _bulletTimeActive = false;
    private bool _freezeFrameActive = false;
    private float _lastTimeScale = 1;

    private void Start()
    {
        EventManager.BulletTime += BulletTime;
        EventManager.FreezeFrame += FreezeFrame;
        EventManager.Pause += PauseTimeChange;
        EventManager.ResetTime += ResetTime;
    }

    private void ChangeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void BulletTime(float newTimeScale)
    {
        if (_bulletTimeActive) return;

        if (_currentTween != null)
            _currentTween.Kill();
        _bulletTimeActive = true;
        _lastTimeScale = Time.timeScale;

        _currentTween = DOVirtual.Float(Time.timeScale, newTimeScale, _bulletTimeTime, (value) => ChangeScale(value))
        .SetEase(Ease.InQuint)
        .OnComplete(() => { _bulletTimeActive = false; });
    }

    private void FreezeFrame()
    {
        if (_freezeFrameActive) return;

        if (_currentTween != null)
            _currentTween.Kill();
        _freezeFrameActive = true;
        _lastTimeScale = Time.timeScale;

        _currentTween = DOVirtual.Float(Time.timeScale, 0.1f, _freezeFrameTime, (value) => ChangeScale(value))
        .SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            ChangeScale(0);
            StartCoroutine("FreezingFrame");
        });
    }

    private IEnumerator FreezingFrame()
    {
        yield return new WaitForSecondsRealtime(_freezeFrameTime);

        ChangeScale(1);
        _freezeFrameActive = false;
    }

    public void PauseTimeChange(bool value)
    {
        if (_currentTween == null) return;

        if (value)
            _currentTween.Pause();
        else
            _currentTween.Play();
    }

    public void ResetTime()
    {
        if (_currentTween != null)
            _currentTween.Kill();

        ChangeScale(1);
    }

    private void OnDestroy()
    {
        EventManager.BulletTime -= BulletTime;
        EventManager.FreezeFrame -= FreezeFrame;
        EventManager.Pause -= PauseTimeChange;
        EventManager.ResetTime -= ResetTime;
    }
}
