using UnityEngine;
using System.Collections.Generic;

public class DamageMarkerManager : MonoBehaviour
{
    // [SerializeField] private Transform _damageMarkersContainers;
    [SerializeField] private List<DamageMarker> _damageMarkers;

    private void Awake()
    {
        foreach (Transform dm in transform)
        {
            if (dm.TryGetComponent(out DamageMarker damageMarker))
            {
                _damageMarkers.Add(damageMarker);
            }
        }

        EventManager.PlayerHit += StartMarker;
    }

    public void StartMarker(Vector3 other)
    {
        DamageMarker _indicator = GetObject();

        if (!_indicator) return;

        _indicator.SetData(other);
        _indicator.gameObject.SetActive(true);
        _indicator.ShowMarker();
    }

    private DamageMarker GetObject()
    {
        for (int i = 0; i < _damageMarkers.Count; i++)
            if (!_damageMarkers[i].gameObject.activeInHierarchy)
                return _damageMarkers[i];

        return null;
    }

    private void OnDestroy()
    {
        EventManager.PlayerHit -= StartMarker;
    }
}
