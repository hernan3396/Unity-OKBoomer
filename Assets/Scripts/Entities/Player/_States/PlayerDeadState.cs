public class PlayerDeadState : PlayerBaseState
{
    private Player _player;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
            _player = stateManager.Player;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        _player.CamAnimator.Play("DeadAnim");
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        return;
    }
}
