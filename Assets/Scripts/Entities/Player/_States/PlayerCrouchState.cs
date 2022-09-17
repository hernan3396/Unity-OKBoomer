using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private PlayerSlide _playerSlide;

    private CapsuleCollider _collider;
    private Transform _crouchHitboxPos;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerLook = _player.PlayerLook;
            _playerSlide = _player.PlayerSlide;
            _collider = _player.SlidingHitbox.GetComponent<CapsuleCollider>();
            _crouchHitboxPos = _collider.transform;
        }
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Dead);

        _playerLook.RotateWeapon();

        if (!Utils.RayHit(_crouchHitboxPos.position, _crouchHitboxPos.position + Vector3.up, "Ceiling", 5))
        {
            if (_player.IsGrounded)
            {
                if (!_playerMovement.IsMoving)
                    stateManager.SwitchState(PlayerStateManager.PlayerState.Idle);
                else
                    stateManager.SwitchState(PlayerStateManager.PlayerState.Run);
            }
        }
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _playerSlide.EndSlide();
    }
}
