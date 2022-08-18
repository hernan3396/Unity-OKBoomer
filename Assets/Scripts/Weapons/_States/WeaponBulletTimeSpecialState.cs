using UnityEngine;

public class WeaponBulletTimeSpecialState : WeaponBaseState
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

        Time.timeScale = _player.CurrentWeaponData.SpecialDamage;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        _utilTimer.StartTimer(_player.CurrentWeaponData.SpecialTime);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(WeaponStateManager.State.Idle);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
