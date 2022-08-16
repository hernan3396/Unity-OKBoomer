using UnityEngine;

public class WeaponCooldownState : WeaponBaseState
{
    private Player _player;
    private float timer;
    private float cooldown;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
            _player = state.Player;

        timer = 0;
        cooldown = _player.CurrentWeaponData.Cooldown;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
            state.SwitchState(WeaponStateManager.State.Idle);
    }
}
