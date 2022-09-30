using System;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity, IPauseable
{
    [SerializeField] private bool _godMode;
    private bool _isDead = false;

    #region Components
    [SerializeField] private PlayerScriptable _data;
    [SerializeField] private PhysicMaterial _noFricMat;
    [SerializeField] private Animator _camAnimator;
    private int _idleCamAnimation;
    private Rigidbody _rb;
    #endregion

    #region Scripts
    private PlayerMovement _playerMovement;
    private PlayerShoot _playerShoot;
    private PlayerSlide _playerSlide;
    private PlayerJump _playerJump;
    private PlayerLook _playerLook;
    private WeaponManager _weaponManager;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _fpCamera;
    [SerializeField] private Transform _slideCamera;
    [SerializeField] private Transform _overlayCamera;
    [SerializeField] private CinemachineImpulseSource _cmImpSrc;
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
    [Header("Weapons")]
    // esto se podria haber hecho como una clase sola pero
    // agregaria un poco de complejidad y ya esta hecho para que funcione asi
    // ademas creo que se entiende la idea ya que todos usan el indice de
    // currentWeapon por lo que no hay problemas para manejarlos
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private int _maxWeapons;
    private int _currentWeapon = 0;
    #endregion

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();

        LoadComponents();

        _currentHp = _data.MaxHealth;
        _invulnerability = _data.Invulnerability;

        SetBullets();

        EventManager.GameStart += GameStart;
        // EventManager.LoadPlayerPos += ;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EventManager.OnGodMode(_godMode);
    }
#endif

    private void GameStart()
    {
        EventManager.OnUpdateUI(UIManager.Element.Hp, _currentHp);
        _weapons[_currentWeapon].UpdateBullets();
        // EventManager.OnUpdateUI(UIManager.Element.Bullets, _bulletsAmount[_currentWeapon]);
    }

    public void SetLoadedInfo(SaveData save)
    {
        _transform.position = save.GetPlayerPosition();
        LoadBullets(save.Ammo);
    }

    private void LoadComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSlide = GetComponent<PlayerSlide>();
        _playerJump = GetComponent<PlayerJump>();
        _playerShoot = GetComponent<PlayerShoot>();
        _playerLook = GetComponent<PlayerLook>();
        _weaponManager = GetComponent<WeaponManager>();

        _idleCamAnimation = Animator.StringToHash("Idle");
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
        if (_godMode || _isDead || _isInmune) return;

        base.TakeDamage(value);
        _cmImpSrc.GenerateImpulse();
        EventManager.OnUpdateUI(UIManager.Element.Hp, _currentHp);
    }

    protected override void Death()
    {
        _isDead = true;
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

    public void Respawn()
    {
        EventManager.OnGameLoad();

        _currentHp = _data.MaxHealth; // este cambiarlo por uno que chequee la save data
        _isDead = false;
        _camAnimator.Play(_idleCamAnimation);

        EventManager.OnStartTransitionOut(_data.DeathDuration);
        EventManager.OnGameStart();
    }
    #endregion

    #region WeaponMethods
    private void SetBullets()
    {
        foreach (Weapon weapon in _weapons)
            weapon.InitialBullets();
    }

    private void LoadBullets(int[] bullets)
    {
        int i = 0;
        foreach (Weapon weapon in _weapons)
        {
            weapon.LoadBullets(bullets[i]);
            i++;
        }
    }

    public void ChangeWeapons(int value)
    {
        _currentWeapon = value;
        _weapons[_currentWeapon].ChangeIn();

        EventManager.OnUpdateUIText(UIManager.Element.Weapon, _weapons[_currentWeapon].Data.Name);
        // EventManager.OnUpdateUI(UIManager.Element.Bullets, _bulletsAmount[_currentWeapon]);
    }

    public void PickUpAmmo(int value)
    {
        // en el caso de armas lo multiplicamos
        // si value = 1, entonces solo le sumas 1/4, si es 2 es 1/2 y asi
        // no hablamos de esto pero lo voy a hacer que agarres 1/4 balas del maximo del arma seleccionada

        foreach (Weapon weapon in _weapons)
        {
            int nextAmmount = (int)(weapon.Data.MaxAmmo * 0.25f) * value;
            weapon.AddBullets(nextAmmount);
        }
    }

    public void PickUpWeapon(int weaponIndex)
    {
        if (weaponIndex <= _maxWeapons)
        {
            PickUpAmmo(1);
            return; // si spameas el nivel en busca de armas esto no te deja agarrar la siguiente
        }

        if (_maxWeapons >= _weapons.Length) return;

        _maxWeapons += 1;
        EventManager.OnPickUpWeapon(1); // 1 indica que sumas 1 al indice del currentWeapon
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

    public Animator CamAnimator
    {
        get { return _camAnimator; }
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

    public WeaponManager WeaponManager
    {
        get { return _weaponManager; }
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

    public Weapon CurrentWeaponData
    {
        get { return _weapons[_currentWeapon]; }
    }

    public int CurrentWeapon
    {
        get { return _currentWeapon; }
    }

    public int MaxWeapons
    {
        get { return _maxWeapons; }
    }

    public bool IsDead
    {
        get { return _isDead; }
    }

    public int[] GetBullets
    {
        get
        {
            int[] ammoCount = new int[3];
            for (int i = 0; i < ammoCount.Length; i++)
            {
                ammoCount[i] = _weapons[i].CurrentBullets;
            }

            return ammoCount;
        }
    }
    #endregion
}