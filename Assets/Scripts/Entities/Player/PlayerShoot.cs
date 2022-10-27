using UnityEngine;
using DG.Tweening;
using Cinemachine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour
{
    private Player _player;
    private PlayerLook _playerLook;
    private PoolManager[] _pools;
    private bool _isShooting = false;
    private bool _isSpecialShooting = false;

    [SerializeField] private CinemachineImpulseSource _cmImpSrc;


    private Vector3 _recoilForce;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Start()
    {
        _pools = GameManager.GetInstance.GetPools;
        _playerLook = _player.PlayerLook;

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
        if (_player.MaxWeapons <= 0) return;

        Weapon weapon = _player.CurrentWeaponData;
        if (weapon.CurrentBullets <= 0) return;

        WeaponScriptable weaponData = weapon.Data;

        GameObject newBullet = _pools[(int)weaponData.AmmoType].GetPooledObject();
        if (!newBullet) return;

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            bullet.SetData(weaponData.Damage, weaponData.AmmoSpeed, weaponData.MaxBounces, weapon.GetShootPos());
            newBullet.SetActive(true);
            bullet.Shoot(weaponData.Accuracy);
            EventManager.OnPlayerPlayerWeaponSound(weaponData.SFX);

            if (!_player.GodMode && weaponData.UseBullets)
                weapon.UseBullets(1);

            // EventManager.OnUpdateUI(UIManager.Element.Bullets, _player.BulletsAmount);
        }

        StartRecoil(weaponData.RecoilForce, weaponData.Cooldown);
    }

    private void StartRecoil(float force, float dur)
    {
        dur *= 0.5f;

        float impulseX = Random.Range(-force, force);
        Vector3 impulseDir = new Vector3(impulseX, force * 2, 0);
        _cmImpSrc.GenerateImpulseWithVelocity(impulseDir);

        // _player.FpCamera.eulerAngles += new Vector3(-force * 10, impulseX * 10, 0);
        _recoilForce = new Vector3(-force * 10, impulseX * 10, 0);
        _playerLook.AddRecoil(_recoilForce);

        _player.Arm.DOLocalMoveZ(-force, dur)
        .SetRelative(true)
        .OnComplete(() => EndRecoil(force, dur));
    }

    private void EndRecoil(float force, float dur)
    {
        _player.Arm.DOLocalMoveZ(force, dur)
        .SetRelative(true);
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

    public Vector3 RecoilForce
    {
        get { return _recoilForce; }
    }
}
