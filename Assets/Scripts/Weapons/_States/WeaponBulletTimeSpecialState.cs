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
        // esto que se repite tanto a lo mejor podria estar en 
        // weapon base state y solo llamar a la funcion
        // aunque eso requiere que las referencias esten en
        // el base state que supongo no esta mal(?)

        if (_playerShoot.IsSpecialShooting)
        {
            _timer += Time.deltaTime;

            if (_timer >= _player.CurrentWeaponData.SpecialTime)
                state.SwitchState(WeaponStateManager.State.CooldownSpecial);
        }
        else
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
