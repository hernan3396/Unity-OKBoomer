using UnityEngine;
using DG.Tweening;

public class WallBossHead : EnemyHead
{
    // fue pensado casi al final asi que es lo que hay
    [SerializeField] private WallBoss _wallBoss;
    [SerializeField] private int _currentHp;
    private bool _isDead = false;
    private Material _mesh;

    private void Start()
    {
        _currentHp = _enemy.Data.Speed;
        _mesh = GetComponent<MeshRenderer>().material;

        EventManager.GameStart += Respawn;
    }

    public override void TakeDamage(int value, Transform other)
    {
        if (_enemy == null || _isDead) return;

        if (_audio != null)
            _audio.PlayOwnSound();

        if (_currentHp - value <= 0)
            value = _currentHp;

        _currentHp -= value;

        _enemy.TakeDamage(value, other);

        if (_currentHp <= 0) DestroyHead();
    }

    private void DestroyHead()
    {
        _isDead = true;

        _wallBoss.HeadDestroyed();

        _mesh.DOFloat(1, "_DissolveValue", 2)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => gameObject.SetActive(false));
    }

    private void Respawn()
    {
        _mesh.SetFloat("_DissolveValue", 0);
        gameObject.SetActive(true);
        _currentHp = _enemy.Data.Speed;
        _isDead = false;
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= Respawn;
    }
}
