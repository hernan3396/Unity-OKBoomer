using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UtilTimer))]
public class StaticEnemy : Enemy
{
    [SerializeField] private Transform _shootingPivot;
    protected PoolManager _bulletsPool;
    private UtilTimer _utilTimer;

    private List<Transform> _shootingPositions = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        _bulletsPool = GameManager.GetInstance.GetEnemyPools[(int)PoolType.SimpleBullet];
        _utilTimer = GetComponent<UtilTimer>();
        _utilTimer.onTimerCompleted += OnTimerCompleted;

        foreach (Transform shoot in _shootingPivot)
            _shootingPositions.Add(shoot);
    }

    public override void Attacking()
    {
        if (!_canAttack) return;

        WeaponScriptable weapon = _data.Weapon;

        foreach (Transform shootPos in _shootingPositions)
        {
            GameObject newBullet = _bulletsPool.GetPooledObject();
            if (!newBullet) return;

            if (newBullet.TryGetComponent(out Bullet bullet))
            {
                Vector3 playerDir = Utils.CalculateDirection(shootPos.position, _player.position);
                shootPos.transform.forward = _lookDir + new Vector3(0, playerDir.y * 0.5f, 0);
                bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, shootPos);
                bullet.SetInitPos(shootPos.position);
                newBullet.SetActive(true);
                bullet.Shoot(weapon.Accuracy);
            }
        }

        _canAttack = false;
        _utilTimer.StartTimer(weapon.Cooldown);
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
