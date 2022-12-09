using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraUtils : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _cmBMCP;
    private bool _isShaking = false;

    private void Awake()
    {
        _cmBMCP = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cmBMCP.m_AmplitudeGain = 0;

        EventManager.CameraShake += StartShake;
    }

    private void StartShake(float intensity, float time)
    {
        if (_isShaking) return;
        _isShaking = true;

        DOTween.To(() => _cmBMCP.m_AmplitudeGain, x => _cmBMCP.m_AmplitudeGain = x, intensity, time)
        .OnComplete(() => FinishShake(time));
    }

    private void FinishShake(float time)
    {
        DOTween.To(() => _cmBMCP.m_AmplitudeGain, x => _cmBMCP.m_AmplitudeGain = x, 0, time)
        .OnComplete(() => { _isShaking = false; });
    }

    private void OnDestroy()
    {
        EventManager.CameraShake -= StartShake;
    }
}
