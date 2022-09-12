using UnityEngine;

public class WeaponChangeOutState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private WeaponManager _weaponManager;
    private UtilTimer _utilTimer;
    private WeaponStateManager _state;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _state = state;
            _player = state.Player;
            _weaponManager = _player.WeaponManager;
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
        _player.GetCurrentModel.SetActive(false);
        _player.ChangeWeapons(_weaponManager.CurrentWeapon);
        _state.SwitchState(WeaponStateManager.State.ChangeIn);
    }
}
