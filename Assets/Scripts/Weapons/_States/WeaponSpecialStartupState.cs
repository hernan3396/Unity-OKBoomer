public class WeaponSpecialStartupState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
            _state = state;

            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_player.CurrentWeaponData.SpecialStartup);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.Idle);
    }

    private void OnTimerCompleted()
    {
        switch (_player.CurrentWeaponData.SpecialType)
        {
            case WeaponScriptable.SpecialAttack.Laser:
                _state.SwitchState(WeaponStateManager.State.LaserSpecial);
                break;
            case WeaponScriptable.SpecialAttack.Explosive:
                _state.SwitchState(WeaponStateManager.State.ExplosiveSpecial);
                break;
            case WeaponScriptable.SpecialAttack.BulletTime:
                _state.SwitchState(WeaponStateManager.State.BulletTimeSpecial);
                break;
        }
    }

    public override void OnExitState(WeaponStateManager state)
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
