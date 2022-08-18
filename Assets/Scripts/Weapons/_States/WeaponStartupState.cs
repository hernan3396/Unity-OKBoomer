public class WeaponStartupState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _state = state;
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;

            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_player.CurrentWeaponData.Startup);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsShooting)
            state.SwitchState(WeaponStateManager.State.Idle);
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(WeaponStateManager.State.Shooting);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
