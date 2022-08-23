public class WeaponIdleState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
        }
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (_playerShoot.IsShooting)
        {
            if (_player.BulletsAmount <= 0 && !_player.GodMode) return;

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
