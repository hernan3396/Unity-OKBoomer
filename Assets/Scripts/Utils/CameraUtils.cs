using UnityEngine;
using DG.Tweening;

public class CameraUtils : MonoBehaviour
{
    #region Components
    private Camera _mainCam;
    #endregion

    [SerializeField] private Ease _ease = Ease.InOutBack;

    private void Awake()
    {
        EventManager.InfiniteRotate += InfiniteRotate;
        _mainCam = GameManager.GetInstance.MainCam;
    }

    public void InfiniteRotate(int speed)
    {
        _mainCam.transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360)
        .SetEase(_ease)
        .SetLoops(-1);
    }

    private void OnDestroy()
    {
        EventManager.InfiniteRotate -= InfiniteRotate;
    }
}
