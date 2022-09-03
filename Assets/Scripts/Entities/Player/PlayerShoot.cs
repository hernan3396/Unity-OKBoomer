using UnityEngine;
using DG.Tweening;
using Cinemachine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour
{
    private Player _player;
    private PoolManager[] _pools;
    private bool _isShooting = false;
    private bool _isSpecialShooting = false;

    [SerializeField] private CinemachineImpulseSource _cmImpSrc;

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

        StartRecoil(weapon.RecoilForce, weapon.Cooldown);

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            bullet.SetData(weapon.Damage, weapon.AmmoSpeed, weapon.MaxBounces, _player.ShootPos);
            newBullet.SetActive(true);
            bullet.Shoot(weapon.Accuracy);

            if (!_player.GodMode && weapon.UseBullets)
                _player.BulletsAmount -= 1;

            EventManager.OnUpdateUI(UIManager.Element.Bullets, _player.BulletsAmount);
        }
    }

    private void StartRecoil(float force, float dur)
    {
        dur *= 0.5f;

        float impulseX = Random.Range(-force, force);
        Vector3 impulseDir = new Vector3(impulseX, force * 2, 0);
        _cmImpSrc.GenerateImpulseWithVelocity(impulseDir);

        _player.FpCamera.eulerAngles += new Vector3(-force * 10, impulseX * 10, 0);

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
}
