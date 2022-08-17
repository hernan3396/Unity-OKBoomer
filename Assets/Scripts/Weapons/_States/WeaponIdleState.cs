public class WeaponIdleState : WeaponBaseState
{
    private PlayerShoot _playerShoot;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_playerShoot == null)
            _playerShoot = state.Player.PlayerShoot;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (_playerShoot.IsShooting)
        {
            state.SwitchState(WeaponStateManager.State.Startup);
            return;
        }

        if (_playerShoot.IsSpecialShooting)
        {
            state.SwitchState(WeaponStateManager.State.StartupSpecial);
            return;
        }
    }
}
