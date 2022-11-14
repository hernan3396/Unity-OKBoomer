public class PlayerRunState : PlayerBaseState
{
    private Player _player;
    private PlayerMovement _playerMovement;
    private UtilTimer _utilTimer;
    private PlayerSlide _playerSlide;
    private PlayerJump _playerJump;
    private PlayerLook _playerLook;
    private bool _canSlide = false;


    public override void OnEnterState(PlayerStateManager stateManager)
    {
        if (_player == null)
        {
            _player = stateManager.Player;
            _playerMovement = _player.PlayerMovement;
            _playerSlide = _player.PlayerSlide;
            _playerJump = _player.PlayerJump;
            _playerLook = _player.PlayerLook;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_player.Data.SlideCooldown);
        _utilTimer.onTimerCompleted += TimerCompleted;
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

        if (_canSlide && _playerSlide.Crouching)
        {
            stateManager.SwitchState(PlayerStateManager.PlayerState.Slide);
            _canSlide = false;
            return;
        }
    }

    public override void FixedUpdateState(PlayerStateManager stateManager)
    {
        _playerMovement.ApplyMovement();
        _playerLook.TiltCamera();
    }

    private void TimerCompleted()
    {
        _canSlide = true;
    }

    public override void OnExitState(PlayerStateManager stateManager)
    {
        _canSlide = false;
        _utilTimer.onTimerCompleted -= TimerCompleted;
    }

    private void OnDestroy()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= TimerCompleted;
    }
}
