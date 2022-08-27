using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity, IPauseable
{
    [SerializeField] private bool _godMode;
    #region Components
    [SerializeField] private PlayerScriptable _data;
    [SerializeField] private PhysicMaterial _noFricMat;
    private Rigidbody _rb;
    #endregion

    #region Scripts
    private PlayerMovement _playerMovement;
    private PlayerShoot _playerShoot;
    private PlayerSlide _playerSlide;
    private PlayerJump _playerJump;
    private PlayerLook _playerLook;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _fpCamera;
    [SerializeField] private Transform _slideCamera;
    [SerializeField] private Transform _overlayCamera;
    #endregion

    #region GroundChecking
    [Header("Ground Checking")]
    [SerializeField, Range(0, 2)] private float _grdDist;
    [SerializeField] private LayerMask _grdLayer;
    [SerializeField] private bool _isGrounded;
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
    [SerializeField] private GameObject[] _hitboxes;
    #endregion

    #region Weapons
    [SerializeField] private List<WeaponScriptable> _weapons = new List<WeaponScriptable>();
    [SerializeField] private Transform _shootPos;
    private int _currentWeapon = 0;
    private int _maxWeapons;
    [SerializeField] private List<int> _bulletsAmount = new List<int>();
    #endregion

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        LoadComponents();

        _currentHp = _data.MaxHealth;
        _invulnerability = _data.Invulnerability;

        _maxWeapons = _weapons.Count;
        SetBullets();

        EventManager.GameStart += GameStart;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EventManager.OnGodMode(_godMode);
    }
#endif

    private void GameStart()
    {
        EventManager.OnGodMode(_godMode);
        EventManager.OnUpdateUI(UIManager.Element.Bullets, _bulletsAmount[_currentWeapon]);
    }

    private void LoadComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSlide = GetComponent<PlayerSlide>();
        _playerJump = GetComponent<PlayerJump>();
        _playerShoot = GetComponent<PlayerShoot>();
        _playerLook = GetComponent<PlayerLook>();
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

    #region DamageMethods
    public void TakeDamage(int value, Vector3 pos)
    {
        if (_godMode) return;
        if (_isInmune) return;

        EventManager.OnPlayerHit(pos);
        TakeDamage(value);
    }

    public override void TakeDamage(int value)
    {
        if (_godMode) return;
        if (_isInmune) return;

        base.TakeDamage(value);
        EventManager.OnUpdateUI(UIManager.Element.Hp, _currentHp);
    }

    protected override void Death()
    {
        EventManager.OnGameOver();
    }

    public void PickUpHealth(int value)
    {
        if (_currentHp + value > _data.MaxHealth)
            _currentHp = _data.MaxHealth;
        else
            _currentHp += value;

        EventManager.OnUpdateUI(UIManager.Element.Hp, _currentHp);
    }
    #endregion

    #region WeaponMethods
    private void SetBullets()
    {
        for (int i = 0; i < _maxWeapons; i++)
            _bulletsAmount.Add(_weapons[i].MaxAmmo);
    }

    public void ChangeWeapons(int value)
    {
        _currentWeapon = value;
        EventManager.OnUpdateUIText(UIManager.Element.Weapon, _weapons[_currentWeapon].Name);
        EventManager.OnUpdateUI(UIManager.Element.Bullets, _bulletsAmount[_currentWeapon]);
    }

    public void PickUpAmmo(int value)
    {
        // en el caso de armas lo multiplicamos
        // si value = 1, entonces solo le sumas 1/4, si es 2 es 1/2 y asi
        // no hablamos de esto pero lo voy a hacer que agarres 1/4 balas del maximo del arma seleccionada
        int nextAmmount = (int)(_weapons[_currentWeapon].MaxAmmo * 0.25f) * value;
        if (BulletsAmount + nextAmmount > _weapons[_currentWeapon].MaxAmmo)
            BulletsAmount = _weapons[_currentWeapon].MaxAmmo;
        else
            BulletsAmount += (int)(_weapons[_currentWeapon].MaxAmmo * 0.25f) * value;

        EventManager.OnUpdateUI(UIManager.Element.Bullets, _bulletsAmount[_currentWeapon]);
    }

    public void PickUpWeapon(WeaponScriptable newWeapon)
    {
        _weapons.Add(newWeapon);

        _bulletsAmount.Add(newWeapon.MaxAmmo);
        _maxWeapons = _weapons.Count;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position + Vector3.down * _grdDist, transform.localScale);
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= GameStart;
    }

    #region Getter/Setter
    public bool GodMode
    {
        get { return _godMode; }
    }

    public Transform Transform
    {
        get { return _transform; }
    }

    public PlayerScriptable Data
    {
        get { return _data; }
    }

    public PhysicMaterial NoFricMat
    {
        get { return _noFricMat; }
    }

    public Rigidbody RB
    {
        get { return _rb; }
    }

    public PlayerMovement PlayerMovement
    {
        get { return _playerMovement; }
    }

    public PlayerSlide PlayerSlide
    {
        get { return _playerSlide; }
    }

    public PlayerJump PlayerJump
    {
        get { return _playerJump; }
    }

    public PlayerShoot PlayerShoot
    {
        get { return _playerShoot; }
    }

    public PlayerLook PlayerLook
    {
        get { return _playerLook; }
    }

    public Transform Body
    {
        get { return _body; }
    }

    public Transform Arm
    {
        get { return _arm; }
    }

    public Transform FpCamera
    {
        get { return _fpCamera; }
    }

    public Transform SlideCamera
    {
        get { return _slideCamera; }
    }

    public Transform OverlayCamera
    {
        get { return _overlayCamera; }
    }

    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    public bool IsFalling
    {
        get { return _rb.velocity.y < 0; }
    }

    public bool Paused
    {
        get { return _isPaused; }
    }

    public GameObject StandingHitbox
    {
        get { return _hitboxes[0]; }
    }

    public GameObject SlidingHitbox
    {
        get { return _hitboxes[1]; }
    }

    public List<WeaponScriptable> GetWeapons
    {
        get { return _weapons; }
    }

    public int CurrentWeapon
    {
        get { return _currentWeapon; }
    }

    public WeaponScriptable CurrentWeaponData
    {
        get { return _weapons[_currentWeapon]; }
    }

    public int MaxWeapons
    {
        get { return _maxWeapons; }
    }

    public Transform ShootPos
    {
        get { return _shootPos; }
    }

    public int BulletsAmount
    {
        get { return _bulletsAmount[_currentWeapon]; }
        set { _bulletsAmount[_currentWeapon] = value; }
    }
    #endregion
}
