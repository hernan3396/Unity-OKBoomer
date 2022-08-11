using UnityEngine;
using DG.Tweening;

public class Platforms : MonoBehaviour
{
    #region Position
    [Header("Position")]
    [SerializeField] private Transform _finalPos;
    #endregion

    #region Settings
    [Header("Settings")]
    [SerializeField] private Ease _easeFunc = Ease.InOutSine;
    [SerializeField] private bool _isDoor = false;
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

        if (_isDoor) return;
        MoveIn();
    }

    public void MoveIn()
    {
        _transform.DOMove(_finalPos.position, _vel)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(_easeFunc)
        .OnComplete(MoveOut);
    }

    public void MoveOut()
    {
        _transform.DOMove(_initPos, _vel)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(_easeFunc)
        .OnComplete(MoveIn);
    }

    public void SimpleMove()
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !_isDoor)
            other.transform.parent = _transform;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !_isDoor)
            other.transform.parent = null;
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}