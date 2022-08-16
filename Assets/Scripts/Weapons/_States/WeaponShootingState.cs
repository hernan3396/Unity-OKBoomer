using UnityEngine;

public class WeaponShootingState : WeaponBaseState
{
    private PlayerShoot _playerShoot;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_playerShoot == null)
            _playerShoot = state.Player.PlayerShoot;

        _playerShoot.Shoot();
        state.SwitchState(WeaponStateManager.State.Cooldown);
    }

    public override void UpdateState(WeaponStateManager state)
    {
        return;
    }
}
