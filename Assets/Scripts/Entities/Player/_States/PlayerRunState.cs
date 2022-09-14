public class PlayerRunState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerSlide _playerSlide;
    private PlayerJump _playerJump;
    private PlayerLook _playerLook;


    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerSlide = _player.PlayerSlide;
            _playerJump = _player.PlayerJump;
            _playerLook = _player.PlayerLook;
        }
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Dead);

        _playerLook.RotateWeapon();

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
}
