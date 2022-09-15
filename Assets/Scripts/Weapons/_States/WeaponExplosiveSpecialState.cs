using UnityEngine;

public class WeaponExplosiveSpecialState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    [SerializeField] private GameObject _explosiveBullet;
    private ExplosiveBullet _explosiveScript;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
            _explosiveScript = _explosiveBullet.GetComponent<ExplosiveBullet>();
        }

        ShootBullet();
        state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    public override void UpdateState(WeaponStateManager state)
    {
        return;
    }

    private void ShootBullet()
    {
        // aca se podria hacer una pool de estas balas
        Weapon weapon = _player.CurrentWeaponData;

        _explosiveScript.ActivateBullet();
        _explosiveScript.SetData((int)weapon.Data.SpecialDamage, weapon.Data.AmmoSpeed, 0, weapon.ShootPos);
        _explosiveScript.Shoot(new Vector2(0, 0));
    }
}
