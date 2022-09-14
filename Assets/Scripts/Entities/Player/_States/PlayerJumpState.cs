using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerJump _playerJump;
    private CapsuleCollider _collider;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerJump = _player.PlayerJump;
            _collider = _player.StandingHitbox.GetComponent<CapsuleCollider>();
        }

        _playerJump.Jump();
        _collider.material = _player.NoFricMat;
        _playerMovement.MovementMod = _player.Data.MovementModifier;
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Dead);

        if (_player.IsFalling)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Fall);
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _collider.material = null;
    }
}
