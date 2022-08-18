using UnityEngine;

public class WeaponStartupState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private float timer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
        }

        timer = 0;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsShooting)
            state.SwitchState(WeaponStateManager.State.Idle);

        timer += Time.deltaTime;

        if (timer >= _player.CurrentWeaponData.Startup)
            state.SwitchState(WeaponStateManager.State.Shooting);
    }
}
