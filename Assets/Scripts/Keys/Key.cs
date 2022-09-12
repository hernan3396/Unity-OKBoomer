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
    [SerializeField] private MeshRenderer _mesh;
    private BoxCollider _col;

    private void Awake()
    {
        _col = GetComponent<BoxCollider>();
    }

    public void DeactivateKey()
    {
        _col.enabled = false;

        _mesh.material.DOFloat(1, "_DissolveValue", 2)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => gameObject.SetActive(false));
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
