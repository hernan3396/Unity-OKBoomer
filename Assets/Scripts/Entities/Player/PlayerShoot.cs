using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour
{
    private Player _player;
    private PoolManager[] _pools;
    private bool _isShooting = false;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        _pools = GameManager.GetInstance.GetPools;

        EventManager.Shoot += ShootInput;
    }

    private void ShootInput(bool value)
    {
        _isShooting = value;
    }

    public void Shoot()
    {
        WeaponScriptable weapon = _player.CurrentWeaponData;

        GameObject newBullet = _pools[(int)weapon.AmmoType].GetPooledObject();
        if (!newBullet) return;

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, _player.ShootPos);
            newBullet.SetActive(true);
            bullet.Shoot(weapon.Accuracy.x, weapon.Accuracy.y);
        }
    }

    private void OnDestroy()
    {
        EventManager.Shoot -= ShootInput;
    }

    public bool IsShooting
    {
        get { return _isShooting; }
    }
}
