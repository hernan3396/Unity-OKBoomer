using UnityEngine;

public class WeaponBulletTimeSpecialState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private float _timer;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
        }

        _timer = 0;

        Time.timeScale = _player.CurrentWeaponData.SpecialDamage;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);

        _timer += Time.deltaTime;

        if (_timer >= _player.CurrentWeaponData.SpecialTime)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
