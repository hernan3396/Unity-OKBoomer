using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    public enum MovementType
    {
        Static,
        Platform,
        Door,
        Key
    }

    #region Position
    [Header("Position")]
    [SerializeField] private Transform _finalPos;
    #endregion

    #region Settings
    [Header("Settings")]
    [SerializeField] private Ease _easeFunc = Ease.InOutSine;
    [SerializeField] private MovementType _type;
    [SerializeField] private float _vel = 1;
    #endregion

    private Transform _transform;
    private Vector3 _initPos;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _initPos = _transform.position;
    }

    private void Start()
    {
        EventManager.Pause += OnPause;

        switch (_type)
        {
            case MovementType.Platform:
                PlatformMovement();
                break;
            case MovementType.Key:
                KeyMovement();
                break;
        }
    }

    public void PlatformMovement()
    {
        _transform.DOMove(_finalPos.position, _vel)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void DoorMovement()
    {
        _transform.DOMove(_finalPos.position, _vel)
        .SetEase(_easeFunc)
        .SetUpdate(UpdateType.Fixed);
    }

    private void OnPause(bool value)
    {
        if (value)
            _transform.DOPause();
        else
            _transform.DOPlay();
    }

    public void KeyMovement()
    {
        // movimiento de rotacion
        _transform.DORotate(new Vector3(0, 360, 0), _vel, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);

        // movimiento vertical
        _transform.DOMove(_finalPos.position, _vel)
        // .SetRelative(true)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _type == MovementType.Platform)
            other.transform.parent = _transform;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _type == MovementType.Platform)
            other.transform.parent = null;
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}
