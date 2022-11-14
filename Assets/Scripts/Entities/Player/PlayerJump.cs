using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour, IPauseable
{
    #region Components
    private Player _player;
    #endregion

    private bool _isJumpign;
    private float _coyoteTimer;
    private float _jumpBufferCounter;

    private bool _jumpPressed;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventManager.Jump += JumpInput;
        EventManager.Pause += OnPause;
    }

    private void Update()
    {

        if (_jumpPressed)
            _jumpBufferCounter = _player.Data.JumpBufferTime;
        else
        {
            // le agrego un chequeo solo para que este valor no siga a menos infinito(?)
            if (_jumpBufferCounter > 0)
                _jumpBufferCounter -= Time.deltaTime;
        }

        if (_player.IsGrounded)
            _coyoteTimer = 0;
        else
            _coyoteTimer += Time.deltaTime;

        _isJumpign = (_jumpBufferCounter > 0 && (_player.IsGrounded || _coyoteTimer < _player.Data.CoyoteMaxTime));
    }

    private void JumpInput(bool value)
    {
        // _isJumpign = value && (_player.IsGrounded || _coyoteTimer < _player.Data.CoyoteMaxTime);
        _jumpPressed = value;
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
