public class PlayerIdleState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private PlayerLook _playerLook;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_playerMovement == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerJump = _player.PlayerJump;
            _playerLook = _player.PlayerLook;
        }
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Dead);

        if (_playerMovement.IsMoving)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Run);
            return;
        }

        if (_playerJump.IsJumping)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Jump);
            return;
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        _playerLook.TiltCamera();
    }
}
