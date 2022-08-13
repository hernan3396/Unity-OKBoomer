public class PlayerRunState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerJump = _player.PlayerJump;
        }
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
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
    }
}
