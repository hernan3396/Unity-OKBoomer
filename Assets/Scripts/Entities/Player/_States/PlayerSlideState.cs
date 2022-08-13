using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    private PlayerMovement _playerMovement;
    private PlayerSlide _playerSlide;
    private Player _player;
    private bool _crouching;
    private float _crouchTimer;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerSlide = _player.PlayerSlide;
            _playerMovement = _player.PlayerMovement;
        }

        _crouching = true;
        _playerSlide.Slide();
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.Paused) return;

        if (_crouching)
        {
            _crouchTimer += Time.deltaTime;

            if (_crouchTimer >= _player.Data.CrouchTimer)
                stateManager.SwitchState(PlayerStateManager.PlayerState.Run);
        }
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _crouchTimer = 0;
        _crouching = false;
        _playerSlide.EndSlide();
    }
}
