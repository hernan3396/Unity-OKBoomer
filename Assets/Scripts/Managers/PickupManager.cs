using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private PoolManager[] _pools;

    private void Start()
    {
        _pools = GameManager.GetInstance.GetPickablesPools;
        EventManager.SpawnPickable += SpawnPickable;
    }

    public void SpawnPickable(Vector3 position)
    {
        GameObject newPickable = _pools[Random.Range(0, _pools.Length)].GetPooledObject();
        if (!newPickable) return;

        newPickable.transform.position = position;
        newPickable.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.SpawnPickable -= SpawnPickable;
    }
}
