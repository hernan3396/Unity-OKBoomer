using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(UtilTimer))]
public class RangedBoss : Enemy
{
    public UnityEvent DeathEvent;

    [SerializeField] private Transform _shootingPivot;
    protected PoolManager _bulletsPool;
    private UtilTimer _utilTimer;
    private int _weaponIndex = 0;
    private PoolManager _explosionPool;

    private List<Transform> _shootingPos = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        _bulletsPool = GameManager.GetInstance.GetEnemyPools[(int)PoolType.TrackerBullet];
        _utilTimer = GetComponent<UtilTimer>();
        _utilTimer.onTimerCompleted += OnTimerCompleted;

        foreach (Transform pos in _shootingPivot)
        {
            _shootingPos.Add(pos);
            pos.GetComponent<MeshRenderer>().enabled = false;
        }

        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    private void OnEnable()
    {
        EventManager.OnStartProgressBar(_data.MaxHealth);
    }

    public override void Attacking()
    {
        if (!_canAttack) return;

        WeaponScriptable weapon = _data.Weapon;
        float timeToWait = weapon.Cooldown;

        _shootingPivot.forward = _lookDir;

        GameObject newBullet = _bulletsPool.GetPooledObject();
        if (!newBullet) return;

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, _shootingPos[_weaponIndex]);
            bullet.SetInitPos(_shootingPos[_weaponIndex].position);
            newBullet.SetActive(true);
            bullet.Shoot(weapon.Accuracy);
            _weaponIndex += 1;

            if (_weaponIndex >= _shootingPos.Count)
            {
                _weaponIndex = 0;
                timeToWait = weapon.Startup;
            }
        }

        _canAttack = false;
        _utilTimer.StartTimer(timeToWait);
    }

    public override void TakeDamage(int value, Transform bullet)
    {
        if (_isInmune || _isDead) return;

        if (!_isDodging)
            _tookDamage = true;

        base.TakeDamage(value, bullet);
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        EventManager.OnUpdateProgressBar(_data.MaxHealth - _currentHp);
    }

    public override void RotateTowards(Transform other)
    {
        _lookDir = Utils.CalculateDirection(_transform.position, _player.position + PredictMovement());
        // _transform.rotation = Quaternion.LookRotation(_lookDir);
        _transform.forward = _lookDir;
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

    private void OnTimerCompleted()
    {
        _canAttack = true;
    }

    protected override void Respawn()
    {
        _currentHp = _data.MaxHealth;
        EventManager.OnDeactivateProgressBar();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
