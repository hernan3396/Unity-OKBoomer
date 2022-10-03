using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private bool _infinite = false;
    private int _currentHealth;

    [Header("Pickup Config")]
    [SerializeField] private bool _spawnSpecificPickable = false;
    [SerializeField] private PickupManager.Pickup _pickupType;
    [SerializeField] private bool _dropPickable = false;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private int _spawnRandomness = 5;
    [SerializeField] private bool _respawn = false;

    [Header("Anim")]
    [SerializeField] private int _fadeDur = 2;
    private BoxCollider _col;
    private Material _mat;

    private void Awake()
    {
        _mat = GetComponentInChildren<MeshRenderer>().material;
        _col = GetComponent<BoxCollider>();
        _spawnPosition.GetComponent<MeshRenderer>().enabled = false;

        _currentHealth = _maxHealth;

        if (_respawn)
            EventManager.GameStart += Respawn;
    }

    public void TakeDamage(int value)
    {
        if (!_infinite)
            _currentHealth -= 1;

        if (_dropPickable) Drop();

        if (_currentHealth <= 0)
            Death();
    }

    private void Death()
    {
        _col.enabled = false;

        _mat.DOFloat(0.5f, "_DissolveValue", _fadeDur)
        .SetEase(Ease.Linear)
        .OnComplete(() => FinishDissolve());
    }

    private void FinishDissolve()
    {
        _mat.DOFloat(1, "_DissolveValue", _fadeDur)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => { gameObject.SetActive(false); });
    }

    private void Drop()
    {
        int randomNum = Random.Range(-_spawnRandomness, _spawnRandomness);
        Vector3 randomVector = new Vector3(randomNum, 0, randomNum);
        Vector3 finalPos = randomVector + _spawnPosition.position;

        if (_spawnSpecificPickable)
            EventManager.OnSpawnSpecificPickable(finalPos, (int)_pickupType);
        else
            EventManager.OnSpawnPickable(finalPos);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        _mat.SetFloat("_DissolveValue", 0);
        _currentHealth = _maxHealth;
        _col.enabled = true;
    }

    private void OnDestroy()
    {
        if (_respawn)
            EventManager.GameStart -= Respawn;
    }
}
