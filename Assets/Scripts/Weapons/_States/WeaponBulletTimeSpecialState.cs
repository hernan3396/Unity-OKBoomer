using UnityEngine;

public class WeaponBulletTimeSpecialState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;

    [SerializeField] private UtilTimer _utilTimerShooting;
    // usamos un segundo timer para disparar, ya que con la logica
    // de las state machine como esta no podriamos disparar
    // desde este estado, entonces voy a "hardcodear" la logica del disparo aca
    private bool _canAttack = true;

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _state = state;
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;

            _utilTimer = GetComponent<UtilTimer>();
        }

        Time.timeScale = _player.CurrentWeaponData.SpecialDamage * 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        _utilTimer.StartTimer(_player.CurrentWeaponData.SpecialTime);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
        _utilTimerShooting.onTimerCompleted += CanAttack;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (_canAttack)
        {
            _playerShoot.Shoot();
            _canAttack = false;
            _utilTimerShooting.StartTimer(_player.CurrentWeaponData.Cooldown);
        }

        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void CanAttack()
    {
        _canAttack = true;
    }

    public override void OnExitState(WeaponStateManager state)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
