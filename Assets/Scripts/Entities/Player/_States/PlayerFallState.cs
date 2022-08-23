using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private Player _player;
    private PlayerJump _playerJump;
    private PlayerMovement _playerMovement;
    private CapsuleCollider _collider;

    #region GravityChange
    private float _gravityTimer;
    private bool _changedGravity;
    #endregion

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerJump = _player.PlayerJump;
            _playerMovement = _player.PlayerMovement;
            _collider = _player.StandingHitbox.GetComponent<CapsuleCollider>();
        }

        if (_playerJump.IsJumping)
            ModifiedGravity();

        _collider.material = _player.NoFricMat;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.IsGrounded)
        {
            if (!_playerMovement.IsMoving)
                stateManager.SwitchState(PlayerStateManager.PlayerState.Idle);
            else
                stateManager.SwitchState(PlayerStateManager.PlayerState.Run);

            _player.ChangeGravity(false); // por si no se reinicio antes la gravedad
        }

        // halves gravity at jump apex
        // slightly notable
        if (_changedGravity)
            GravityTimer();
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _collider.material = null;
        _playerMovement.MovementMod = 1;
    }

    private void ModifiedGravity()
    {
        _changedGravity = true;
        _gravityTimer = 0;
        _player.ChangeGravity(true);
    }

    private void GravityTimer()
    {
        _gravityTimer += Time.deltaTime;

        if (_gravityTimer >= _player.Data.HalfGravityLimit)
        {
            _changedGravity = false;
            _player.ChangeGravity(false);
        }
    }
}
