using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CoreEnemy : Enemy
{
    public UnityEvent DeathEvent;
    [SerializeField] private float _intensityStart = 0.15f;
    [SerializeField] private Color _endingColor;
    private PoolManager _explosionPool;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    public override void Attacking()
    {
        return;
    }

    public override void TakeDamage(int value)
    {
        if (_isInmune || _isDead) return;

        base.TakeDamage(value);
        float intensityValue = (_data.MaxHealth - _currentHp) * 0.01f;

        _mainMat.SetFloat("_Intensity", _intensityStart + intensityValue);
    }

    public void Dying()
    {
        EventManager.OnBulletTime(0.5f);

        if (_audio != null)
            _audio.PlaySound((int)SFX.Death);

        _headMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine);

        _mainMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine)
        .OnComplete(() =>
        {
            EventManager.OnResetTime();
            DeathEvent?.Invoke();
        });
    }

    public void CreateExplosion()
    {
        GameObject explosion = _explosionPool.GetPooledObject();

        if (!explosion) return;
        Vector3 explosionPos = _transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = explosionPos;
            explosion.SetActive(true);
            explosionScript.HarmlessExplosion(_data.DodgeRange, _data.DodgeSpeed);
        }
    }

    protected override void Death()
    {
        if (_isDead) return;

        _isDead = true;
        _col.enabled = false;
    }
}
