using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private Player _player;

    private float _animDuration;
    private int _deadAnim;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _deadAnim = Animator.StringToHash("DeadAnim");
        }

        _player.CamAnimator.Play(_deadAnim);
        EventManager.OnStartTransition(_player.Data.DeathDuration);
        EventManager.OnGameOver();
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (!_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Idle);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        return;
    }
}
