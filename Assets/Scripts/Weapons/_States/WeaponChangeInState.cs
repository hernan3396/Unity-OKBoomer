using UnityEngine;

public class WeaponChangeInState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private UtilTimer _utilTimer;
    private WeaponStateManager _state;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _state = state;
            _player = state.Player;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_player.CurrentWeaponData.ChangeDur);
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
}
