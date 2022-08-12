using UnityEngine;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        Blue,
        Green,
        Red
    }

    [SerializeField] private KeyType _keyType;

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
