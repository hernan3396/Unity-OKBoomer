using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private Transform _position;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Bullet"))
            Spawn();
    }

    public void Spawn()
    {
        Instantiate(_object, _position);
    }
}
