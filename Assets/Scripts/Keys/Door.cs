using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Door : MonoBehaviour
{
    // aca chequear si el player tiene la llave
    [SerializeField] private Key.KeyType _keyType;
    private Movement _movement;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    public void Open()
    {
        _movement.DoorMovement();
    }

    public Key.KeyType GetKeyType
    {
        get { return _keyType; }
    }
}
