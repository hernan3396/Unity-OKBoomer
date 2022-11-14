using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    // este deberia tener movmientos simples, como ir hacia un lado, rotar,etc.
    // y luego tener scripts separados de cada cosa especifica como la llave, elevador, etc.
    // y que llamen al movimiento que queres hacer, en vez de como hice aca que estan las cosas mezcladas
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
    [SerializeField] private bool _autoStart = true;
    [SerializeField] private MovementType _type;
    [SerializeField] private float _vel = 1;
    private GameObject _player;
    #endregion

    private Transform _transform;
    private Vector3 _initPos;

    private Tween _rotationTween;
    private Tween _movementTween;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        EventManager.Pause += OnPause;
        _player = GameManager.GetInstance.Player;

        if (!_autoStart) return;

        Initialize();
    }

    private void Initialize()
    {
        _initPos = _transform.position;
        _finalPosition = _finalPos.position;

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
        _rotationTween = _transform.DORotate(new Vector3(0, 360, 0), _vel, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(_easeFunc)
        .SetLoops(-1, LoopType.Yoyo);

        // movimiento vertical
        _movementTween = _transform.DOMove(_finalPosition, _vel)
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

    public void StartMovement()
    {
        Initialize();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _type == MovementType.Elevator)
        {
            ElevatorUp();
            _player.transform.parent = _transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _type == MovementType.Elevator)
        {
            _player.transform.parent = null;
            ElevatorDown();
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

    private void OnDisable()
    {
        // se supone que si uno es != nulo, el otro tambien
        if (_rotationTween != null)
        {
            _rotationTween.Kill();
            _movementTween.Kill();
        }
    }
}
