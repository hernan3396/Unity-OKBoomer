public class PlayerIdleState : PlayerBaseState
{
    private PlayerMovement _playerMovement;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_playerMovement == null)
            _playerMovement = stateManager.Player.PlayerMov;

        // if (_playerJump == null)
        //     _playerJump = stateManager.Player.PlayerJump;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_playerMovement.IsMoving)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Run);
            return;
        }

        // if (_playerJump.IsJumping)
        // {
        //     stateManager.SwitchState(PlayerStateManager.PlayerState.Jump);
        //     return;
        // }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        return;
    }
}
