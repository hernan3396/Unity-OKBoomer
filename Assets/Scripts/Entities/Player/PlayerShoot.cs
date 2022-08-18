using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour
{
    private Player _player;
    private PoolManager[] _pools;
    private bool _isShooting = false;
    private bool _isSpecialShooting = false;

    // [SerializeField] private int[] _bulletsAmount = new int[3];

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        _pools = GameManager.GetInstance.GetPools;

        EventManager.Shoot += ShootInput;
        EventManager.SpecialShoot += SpecialShootInput;
    }

    private void ShootInput(bool value)
    {
        _isShooting = value;
    }

    private void SpecialShootInput(bool value)
    {
        _isSpecialShooting = value;
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

            if (!_player.GodMode)
                _player.BulletsAmount -= 1;

            EventManager.OnUpdateUI(UIManager.Element.Bullets, _player.BulletsAmount);
        }
    }

    private void OnDestroy()
    {
        EventManager.Shoot -= ShootInput;
        EventManager.SpecialShoot -= SpecialShootInput;
    }

    public bool IsShooting
    {
        get { return _isShooting; }
    }

    public bool IsSpecialShooting
    {
        get { return _isSpecialShooting; }
    }
}
