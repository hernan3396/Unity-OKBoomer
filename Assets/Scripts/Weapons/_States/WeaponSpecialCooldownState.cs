using UnityEngine;

public class WeaponSpecialCooldownState : WeaponBaseState
{
    private Player _player;
    private float _timer;
    private float _cooldown;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
            _player = state.Player;

        _timer = 0;
        _cooldown = _player.CurrentWeaponData.SpecialCooldown;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        _timer += Time.deltaTime;

        if (_timer >= _cooldown)
            state.SwitchState(WeaponStateManager.State.Idle);
    }
}
