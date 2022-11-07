using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UtilTimer))]
public class RangedBoss : Enemy
{
    // es practicamente igual al static enemy hago esto por 
    // si me da el tiempo de crearle un comportamiento distinto,
    // mas de boss, asi no hay que cambiar tanto luego si se llega a hacer.
    [SerializeField] private Transform _shootingPivot;
    protected PoolManager _bulletsPool;
    private UtilTimer _utilTimer;

    private List<Transform> _shootingPos = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        _bulletsPool = GameManager.GetInstance.GetEnemyPools[(int)PoolType.SimpleBullet];
        _utilTimer = GetComponent<UtilTimer>();
        _utilTimer.onTimerCompleted += OnTimerCompleted;

        foreach (Transform pos in _shootingPivot)
        {
            _shootingPos.Add(pos);
            pos.GetComponent<MeshRenderer>().enabled = false;
        }

        EventManager.OnStartProgressBar(_data.MaxHealth);
    }

    public override void Attacking()
    {
        if (!_canAttack) return;

        WeaponScriptable weapon = _data.Weapon;

        foreach (Transform shootPos in _shootingPos)
        {
            GameObject newBullet = _bulletsPool.GetPooledObject();
            if (!newBullet) return;

            if (newBullet.TryGetComponent(out Bullet bullet))
            {
                shootPos.transform.forward = _lookDir;
                bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, shootPos);
                bullet.SetInitPos(shootPos.position);
                newBullet.SetActive(true);
                bullet.Shoot(weapon.Accuracy);
            }
        }

        _canAttack = false;
        _utilTimer.StartTimer(weapon.Cooldown);
    }

    public override void TakeDamage(int value, Transform bullet)
    {
        if (_isInmune || _isDead) return;

        if (!_isDodging)
            _tookDamage = true;

        base.TakeDamage(value, bullet);
        EventManager.OnUpdateProgressBar(_data.MaxHealth - _currentHp);
    }

    public override void RotateTowards(Transform other)
    {
        _lookDir = Utils.CalculateDirection(_shootingPivot.position, _player.position + PredictMovement());
        _transform.rotation = Quaternion.LookRotation(_lookDir);
    }

    private void OnTimerCompleted()
    {
        _canAttack = true;
    }

    private void OnDestroy()
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
