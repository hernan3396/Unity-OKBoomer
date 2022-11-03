using UnityEngine;

[RequireComponent(typeof(UtilTimer))]
public class RangedEnemy : Enemy
{
    [SerializeField] private Transform _shootingPos;
    protected PoolManager _bulletsPool;
    private UtilTimer _utilTimer;

    override protected void Start()
    {
        base.Start();
        _bulletsPool = GameManager.GetInstance.GetEnemyPools[(int)PoolType.SimpleBullet];
        _utilTimer = GetComponent<UtilTimer>();
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void Attacking()
    {
        if (!_canAttack) return;

        WeaponScriptable weapon = _data.Weapon;

        GameObject newBullet = _bulletsPool.GetPooledObject();
        if (!newBullet) return;

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, _shootingPos);
            bullet.SetInitPos(_shootingPos.position);
            newBullet.SetActive(true);
            bullet.Shoot(weapon.Accuracy);
        }

        _canAttack = false;
        _utilTimer.StartTimer(weapon.Cooldown);
    }

    private void OnTimerCompleted()
    {
        _canAttack = true;
    }

    // public override void RotateTowards(Transform other)
    // {
    //     _lookDir = Utils.CalculateDirection(_shootingPos.position, _player.position + PredictMovement());
    //     _transform.rotation = Quaternion.LookRotation(_lookDir);
    // }

    private void OnDestroy()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
