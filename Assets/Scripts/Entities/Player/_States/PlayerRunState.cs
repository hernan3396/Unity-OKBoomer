using DG.Tweening;
using UnityEngine;
using Cinemachine;

public class PlayerRunState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerSlide _playerSlide;
    private PlayerJump _playerJump;

    [SerializeField] private Transform _fpCamera;

    private CinemachineImpulseSource _cmImpSrc;
    private UtilTimer _utilTimer;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerSlide = _player.PlayerSlide;
            _playerJump = _player.PlayerJump;

            _utilTimer = GetComponent<UtilTimer>();
            _cmImpSrc = GetComponent<CinemachineImpulseSource>();
        }

        _utilTimer.StartTimer(0.5f);
        _utilTimer.onTimerCompleted += Bump;
    }

    // private void CameraShake()
    // {
    //     if (_fpCamera != null)
    //         _fpCamera.DOShakeRotation(2f, 10, 0, 70)
    //         .SetId(_fpCamera)
    //         .OnComplete(() => CameraShake());
    // }

    private void Bump()
    {
        _cmImpSrc.GenerateImpulse();
        _utilTimer.StartTimer(0.5f);
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (!_playerMovement.IsMoving)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Idle);
            return;
        }

        if (_playerJump.IsJumping)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Jump);
            return;
        }

        if (_player.RB.velocity.magnitude > 1 && _playerSlide.Crouching)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Slide);
            return;
        }
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        DOTween.Kill(_fpCamera, true);
        _utilTimer.onTimerCompleted -= Bump;
    }
}
