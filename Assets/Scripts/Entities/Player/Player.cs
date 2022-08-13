using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity, IPauseable
{
    #region Components
    [SerializeField] private PlayerScriptable _data;
    [SerializeField] private PhysicMaterial _noFricMat;
    private Rigidbody _rb;
    #endregion

    #region Scripts
    private PlayerMovement _playerMovement;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    #endregion

    #region GroundChecking
    [Header("Ground Checking")]
    [SerializeField, Range(0, 1)] private float _grdDist;
    [SerializeField] private LayerMask _grdLayer;
    private bool _isGrounded;
    bool _hitDetect;
    RaycastHit _hit;
    #endregion

    #region Gravity
    [Header("Gravity")]
    [SerializeField] private float _gravityMod = 1;
    #endregion

    #region Pause
    private bool _isPaused = false;
    private Vector3 _lastVel;
    #endregion

    #region Hitboxes
    [SerializeField] private Transform[] _cameraPositions;
    [SerializeField] private GameObject[] _hitboxes;
    #endregion

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        LoadComponents();

        _currentHp = _data.MaxHealth;
        _invulnerability = _data.Invulnerability;
    }

    private void LoadComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;

        _rb.AddForce(Physics.gravity * _data.Gravity * _gravityMod, ForceMode.Acceleration);

        // _rb.velocity = _utilsMov.LimitFallSpeed(_rb.velocity, _data.FallMaxSpeed);
        if (_rb.velocity.y < _data.MaxFallSpeed)
            _rb.velocity = new Vector3(_rb.velocity.x, _data.MaxFallSpeed, _rb.velocity.z);

        _isGrounded = GroundChecking();
    }

    #region GroundCheckingMethods
    private bool GroundChecking()
    {
        _hitDetect = Physics.BoxCast(_transform.position, _transform.localScale, Vector3.down, out _hit, _transform.rotation, _grdDist, _grdLayer);

        if (_hitDetect)
            return true;

        return false;
    }
    #endregion

    #region GravityMethods
    /// <Summary>
    ///  if true halves gravity, false returns it
    /// </Summary>
    public void ChangeGravity(bool change)
    {
        // estos valores supongo se pueden hacer configurables
        if (change)
            _gravityMod = 0.5f;
        else
            _gravityMod = 1;
    }
    #endregion

    #region PauseMethods
    public void OnPause(bool value)
    {
        _isPaused = value;

        if (_isPaused)
            PausePlayer();
        else
            ResumePlayer();
    }

    private void PausePlayer()
    {
        _lastVel = _rb.velocity;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
    }

    private void ResumePlayer()
    {
        _rb.velocity = _lastVel;
        _rb.useGravity = true;
    }
    #endregion

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
    }

    protected override void Death()
    {
        EventManager.OnGameOver();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position + Vector3.down * _grdDist, transform.localScale);
    }

    #region Getter/Setter
    public Transform Transform
    {
        get { return _transform; }
    }

    public PlayerScriptable Data
    {
        get { return _data; }
    }

    public Rigidbody RB
    {
        get { return _rb; }
    }

    public PlayerMovement PlayerMov
    {
        get { return _playerMovement; }
    }

    public bool Paused
    {
        get { return _isPaused; }
    }
    #endregion
}
