using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    private PlayerMovement _playerMovement;
    private PlayerSlide _playerSlide;
    private CapsuleCollider _collider;
    private Transform _crouchHitboxPos;
    private Player _player;
    private bool _crouching;
    private float _crouchTimer;

    private bool _canStand = true;

    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerSlide = _player.PlayerSlide;
            _playerMovement = _player.PlayerMovement;
            _collider = _player.SlidingHitbox.GetComponent<CapsuleCollider>();
            _crouchHitboxPos = _collider.transform;
        }

        _crouching = true;
        _playerSlide.Slide();
        _collider.material = _player.NoFricMat;
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (_player.Paused) return;

        if (_player.IsDead)
            stateManager.SwitchState(PlayerStateManager.PlayerState.Dead);

        if (_crouching)
        {
            _crouchTimer += Time.deltaTime;

            if (_crouchTimer >= _player.Data.CrouchTimer)
            {
                if (Utils.RayHit(_crouchHitboxPos.position, _crouchHitboxPos.position + Vector3.up, "Floor", 5, _player.Data.CeilingLayer))
                {
                    Debug.Log("Agachado");
                    _canStand = false;
                    stateManager.SwitchState(PlayerStateManager.PlayerState.Crouch);
                    return;
                }

                stateManager.SwitchState(PlayerStateManager.PlayerState.Run);
            }
        }
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _crouchTimer = 0;
        _crouching = false;
        _collider.material = null;

        if (_canStand)
            _playerSlide.EndSlide();

        _canStand = true;
    }
}
