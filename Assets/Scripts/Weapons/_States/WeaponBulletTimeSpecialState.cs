using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class WeaponBulletTimeSpecialState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;
    private PlayerMovement _playerMovement;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;

    [SerializeField] private UtilTimer _utilTimerShooting;
    [SerializeField] private Volume _volume;
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
            _playerMovement = _player.PlayerMovement;

            _utilTimer = GetComponent<UtilTimer>();
        }

        _playerMovement.MovementMod = 2;
        EventManager.OnBulletTime(_player.CurrentWeaponData.Data.SpecialDamage * 0.1f);

        DOTween.To(() => _volume.weight, x => _volume.weight = x, 1, 1)
        .SetUpdate(true);

        _utilTimer.StartTimer(_player.CurrentWeaponData.Data.SpecialTime);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
        _utilTimerShooting.onTimerCompleted += CanAttack;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        _playerMovement.MovementMod = 2;

        if (_canAttack && _player.CurrentWeaponData.CurrentBullets > 0)
        {
            _playerShoot.Shoot();
            _canAttack = false;
            _utilTimerShooting.StartTimer(_player.CurrentWeaponData.Data.Cooldown);
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
        _playerMovement.MovementMod = 1;
        DOTween.To(() => _volume.weight, x => _volume.weight = x, 0, 1)
.SetUpdate(true);

        EventManager.OnBulletTime(1);
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
