using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour, IPauseable
{
    #region Components
    private Player _player;
    #endregion

    private bool _isJumpign;
    private float _coyoteTimer;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventManager.Jump += JumpInput;
        EventManager.Pause += OnPause;
    }

    private void JumpInput(bool value)
    {
        _isJumpign = value;
    }

    public void Jump()
    {
        if (_player.Paused) return;
        _player.RB.velocity = new Vector3(_player.RB.velocity.x, _player.Data.JumpStrength, _player.RB.velocity.z);
    }

    public void OnPause(bool value)
    {
        if (value)
            _isJumpign = false;
    }

    private void OnDestroy()
    {
        EventManager.Jump -= JumpInput;
        EventManager.Pause -= OnPause;
    }

    public bool IsJumping
    {
        get { return _isJumpign; }
    }
}
