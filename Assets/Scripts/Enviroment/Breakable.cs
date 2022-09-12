using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour, IDamageable
{
    [SerializeField] private bool _dropPickable = false;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private int _fadeDur = 2;
    [SerializeField] private bool _spawnSpecificPickable = false;
    [SerializeField] private PickupManager.Pickup _pickupType;
    private Material _mat;
    private BoxCollider _col;

    private void Awake()
    {
        _mat = GetComponentInChildren<MeshRenderer>().material;
        _col = GetComponentInChildren<BoxCollider>();
        _spawnPosition.GetComponent<MeshRenderer>().enabled = false;
    }

    public void TakeDamage(int value)
    {
        Death();
    }

    private void Death()
    {
        _col.enabled = false;

        _mat.DOFloat(0.5f, "_DissolveValue", _fadeDur)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            FinishDissolve();
            if (!_dropPickable) return;

            if (_spawnSpecificPickable)
                EventManager.OnSpawnSpecificPickable(_spawnPosition.position, (int)_pickupType);
            else
                EventManager.OnSpawnPickable(_spawnPosition.position);
        });
    }

    private void FinishDissolve()
    {
        _mat.DOFloat(1, "_DissolveValue", _fadeDur)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => { gameObject.SetActive(false); });
    }
}
