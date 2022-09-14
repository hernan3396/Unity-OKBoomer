using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    public enum MovementType
    {
        Static,
        Platform,
        Elevator,
        Door,
        Key,
    }

    #region Position
    [Header("Position")]
    [SerializeField] private Transform _finalPos;
    private Vector3 _finalPosition; // esta es a la que te moves
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
        _finalPosition = _finalPos.position;
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
        _transform.DOMove(_finalPosition, _vel)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void DoorMovement()
    {
        _transform.DOMove(_finalPosition, _vel)
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

    private void KeyMovement()
    {
        // movimiento de rotacion
        _transform.DORotate(new Vector3(0, 360, 0), _vel, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);

        // movimiento vertical
        _transform.DOMove(_finalPosition, _vel)
        // .SetRelative(true)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);
    }

    private void ElevatorUp()
    {
        _transform.DOMove(_finalPosition, _vel)
        .SetEase(_easeFunc)
        .SetUpdate(UpdateType.Fixed);
    }

    private void ElevatorDown()
    {
        _transform.DOMove(_initPos, _vel)
        .SetEase(_easeFunc)
        .SetUpdate(UpdateType.Fixed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _type == MovementType.Platform)
            other.transform.parent = _transform;

        if (other.gameObject.CompareTag("Player") && _type == MovementType.Elevator)
        {
            ElevatorUp();
            other.transform.parent = _transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && _type == MovementType.Platform)
            other.transform.parent = null;

        if (other.gameObject.CompareTag("Player") && _type == MovementType.Elevator)
        {
            other.transform.parent = null;
            ElevatorDown();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _finalPos.position);
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}
