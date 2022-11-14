using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public enum Pickup
    {
        Health,
        Ammo
    }

    private PoolManager[] _pools;

    private void Start()
    {
        _pools = GameManager.GetInstance.GetPickablesPools;
        EventManager.SpawnPickable += SpawnPickable;
        EventManager.SpawnSpecificPickable += SpawnSpecificPickable;
    }

    public void SpawnPickable(Vector3 position)
    {
        GameObject newPickable = _pools[Random.Range(0, _pools.Length)].GetPooledObject();
        if (!newPickable) return;

        newPickable.transform.position = position;
        newPickable.SetActive(true);
    }

    public void SpawnSpecificPickable(Vector3 position, int value)
    {
        GameObject newPickable = _pools[value].GetPooledObject();
        if (!newPickable) return;

        newPickable.transform.position = position;
        newPickable.SetActive(true);

        if (newPickable.TryGetComponent(out Movement movement))
            movement.StartMovement();
    }

    private void OnDestroy()
    {
        EventManager.SpawnPickable -= SpawnPickable;
        EventManager.SpawnSpecificPickable -= SpawnSpecificPickable;
    }
}
