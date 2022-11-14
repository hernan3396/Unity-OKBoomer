public class WeaponSpecialCooldownState : WeaponBaseState
{
    private Player _player;
    private float _cooldown;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;

            _state = state;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _player.CurrentWeaponData.CooldownAnim();
        _cooldown = _player.CurrentWeaponData.Data.SpecialCooldown;
        _utilTimer.StartTimer(_cooldown);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        return;
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(WeaponStateManager.State.Idle);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
