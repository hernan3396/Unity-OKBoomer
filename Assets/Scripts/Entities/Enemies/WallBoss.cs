using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(UtilTimer))]
public class WallBoss : Enemy
{
    public UnityEvent DeathEvent;
    [SerializeField] private Transform _finalPos;
    private PoolManager _explosionPool;
    private Tween _movingTween;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    private void OnEnable()
    {
        EventManager.OnStartProgressBar(_data.MaxHealth);
    }

    public override void Attacking()
    {
        _movingTween = _transform.DOMove(_finalPos.position, 50)
        .SetEase(Ease.OutSine);
    }

    public void Hurt()
    {
        // frena al jefe
        if (_movingTween != null)
            _movingTween.Kill();
        // luego dispara lasers
    }

    public void ShootLasers()
    {
        // disparar lasers
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        EventManager.OnUpdateProgressBar(_data.MaxHealth - _currentHp);
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
        Vector3 explosionPos = _headPos.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

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
    }

    protected override void Respawn()
    {
        _currentHp = _data.MaxHealth;
        EventManager.OnDeactivateProgressBar();
        gameObject.SetActive(false);
    }
}
