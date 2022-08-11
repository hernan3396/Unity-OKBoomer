using UnityEngine;
using System.Collections.Generic;

public class KeyHolder : MonoBehaviour
{
    [SerializeField] private List<Key.KeyType> _keyList = new List<Key.KeyType>();

    public void AddKey(Key.KeyType keyType)
    {
        _keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        _keyList.Remove(keyType);
    }

    public bool HasKey(Key.KeyType keyType)
    {
        return _keyList.Contains(keyType);
    }

    private void UseKey(Door door)
    {
        if (!HasKey(door.GetKeyType)) return;

        RemoveKey(door.GetKeyType);
        door.Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Door door))
        {
            UseKey(door);
            return;
        }

        if (other.TryGetComponent(out Key key))
        {
            AddKey(key.GetKeyType);
            key.DeactivateKey();
            return;
        }
    }
}
