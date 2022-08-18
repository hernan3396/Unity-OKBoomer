using UnityEngine;

public class WeaponSpecialStartupState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private float _timer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
        }

        _timer = 0;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.Idle);

        _timer += Time.deltaTime;

        if (_timer < _player.CurrentWeaponData.SpecialStartup) return;

        switch (_player.CurrentWeaponData.SpecialType)
        {
            case WeaponScriptable.SpecialAttack.Laser:
                state.SwitchState(WeaponStateManager.State.LaserSpecial);
                break;
            case WeaponScriptable.SpecialAttack.Explosive:
                state.SwitchState(WeaponStateManager.State.ExplosiveSpecial);
                break;
            case WeaponScriptable.SpecialAttack.BulletTime:
                state.SwitchState(WeaponStateManager.State.BulletTimeSpecial);
                break;
        }


    }
}
