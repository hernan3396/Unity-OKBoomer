using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        Blue,
        Green,
        Red
    }

    [SerializeField] private KeyType _keyType;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        // pasar este movimiento a movement.cs
        // movimiento de rotacion
        _transform.DORotate(new Vector3(0, 360, 0), 3, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Yoyo);

        // movimiento vertical
        _transform.DOMoveY(1, 3)
        .SetRelative(true)
        .SetEase(Ease.OutBounce)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void DeactivateKey()
    {
        gameObject.SetActive(false);
        // aca hacer la animacion con disolve
    }

    private void ChangeColor()
    {
        // aca cambiar el shader??
    }

    public KeyType GetKeyType
    {
        get { return _keyType; }
    }
}
