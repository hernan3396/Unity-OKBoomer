public class WeaponIdleState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    private WeaponStateManager _state;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
            _state = state;
        }

        EventManager.ChangeWeapon += ChangeWeapon;
        EventManager.PickUpWeapon += ChangeWeapon;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (_player.GetWeapons.Count == 0) return;

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

    public override void OnExitState(WeaponStateManager state)
    {
        EventManager.ChangeWeapon -= ChangeWeapon;
        EventManager.PickUpWeapon -= ChangeWeapon;
    }

    private void ChangeWeapon(int side)
    {
        if (_player.GetWeapons.Count == 0) return;

        _player.WeaponManager.ChangeWeapon(side);
        _state.SwitchState(WeaponStateManager.State.ChangeOut);
    }
}
