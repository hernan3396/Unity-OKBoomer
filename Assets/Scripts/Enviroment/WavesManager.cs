using UnityEngine;
using UnityEngine.Events;

public class WavesManager : MonoBehaviour
{
    public UnityEvent WaveFinished;

    [SerializeField] private int _requiredAmount = 1;
    private int _currentAmount = 0;

    private void OnEnable()
    {
        EventManager.WaveUpdated += Counting;
    }

    private void Counting()
    {
        _currentAmount += 1;

        if (_currentAmount >= _requiredAmount)
            WaveFinished?.Invoke();
    }

    private void OnDisable()
    {
        EventManager.WaveUpdated -= Counting;
    }

    private void OnDestroy()
    {
        EventManager.WaveUpdated -= Counting;
    }

    public void ResetWave()
    {
        _currentAmount = 0;
    }
}
