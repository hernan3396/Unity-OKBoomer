using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(UtilTimer))]
public class BulletHellBoss : Enemy
{
    public UnityEvent DeathEvent;

    [Header("Config")]
    [SerializeField] private GameObject _bossParent;
    [SerializeField] private Transform _headPivot;
    [SerializeField] private Transform _waypointsPivot;
    [SerializeField] private Transform _shootingPivot;
    [SerializeField] private Transform _laserPivot;
    [SerializeField] private Transform _jumpableLaserPivot;
    [SerializeField] private Transform _slideableLaserPivot;
    private Vector3 _initPos;
    // Components
    [SerializeField] private PoolManager _bulletsPool;
    private PoolManager _explosionPool;
    private WeaponScriptable _weapon;
    private UtilTimer _utilTimer;

    // lists
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    private List<Transform> _shootingPos = new List<Transform>();
    private List<Transform> _jumpableLasers = new List<Transform>();
    private List<Transform> _slideableLasers = new List<Transform>();

    private bool _isMoving = false;
    private bool _isLasering = false;

    protected override void Start()
    {
        base.Start();

        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);

        _utilTimer = GetComponent<UtilTimer>();
        _utilTimer.onTimerCompleted += OnTimerCompleted;
        _weapon = _data.Weapon;
        _initPos = _transform.position;

        FillLists();
    }

    private void OnEnable()
    {
        EventManager.OnStartProgressBar(_data.MaxHealth);
    }

    private void FillLists()
    {
        foreach (Transform waypoint in _waypointsPivot)
        {
            _waypoints.Add(waypoint);
            waypoint.GetComponent<MeshRenderer>().enabled = false;
        }

        foreach (Transform shootingPos in _shootingPivot)
        {
            _shootingPos.Add(shootingPos);
            shootingPos.GetComponent<MeshRenderer>().enabled = false;
        }

        foreach (Transform laser in _jumpableLaserPivot)
            _jumpableLasers.Add(laser);

        foreach (Transform laser in _slideableLaserPivot)
            _slideableLasers.Add(laser);
    }

    public override void Attacking()
    {
        return;
    }

    #region Behaviour
    public void ShakeEye()
    {
        // cuando sufre x cantidad de daño
        _headPivot.DOShakeRotation(1, 90, 10, 90);
    }

    public void MoveToWaypoint()
    {
        _isMoving = true;
        int randPos = Random.Range(0, _waypoints.Count);

        _transform.DOJump(_waypoints[randPos].position, _data.Speed, _data.WalkPointRange, _data.Acceleration)
        .SetEase(Ease.OutSine)
        .OnComplete(() => { _isMoving = false; });
    }

    public void StartShooting()
    {
        if (!_canAttack) return;

        float timeToWait = _weapon.Cooldown;
        Invoke("Shoot", _weapon.Startup);

        _canAttack = false;
        _utilTimer.StartTimer(timeToWait);
    }

    private void Shoot()
    {
        _shootingPivot.forward = _lookDir;

        foreach (Transform shootingPos in _shootingPos)
        {
            GameObject newBullet = _bulletsPool.GetPooledObject();
            if (!newBullet) return;

            if (newBullet.TryGetComponent(out Bullet bullet))
            {
                bullet.SetData(_weapon.Damage, _weapon.AmmoSpeed, _weapon.MaxBounces, shootingPos);
                bullet.SetInitPos(shootingPos.position);
                newBullet.SetActive(true);
                bullet.Shoot(_weapon.Accuracy);
            }
        }
    }

    public void Lasers()
    {
        StopCoroutine(ShootingLasers());
        StartCoroutine(ShootingLasers());
    }

    private IEnumerator ShootingLasers()
    {
        // lord forgive me for what im about to do
        int initLasers = Random.Range(0, 2);

        if (initLasers == 0)
        {
            _transform.DOJump(_laserPivot.position, _data.Speed, _data.WalkPointRange, _data.Acceleration * 0.5f)
            .OnComplete(() => ShootLasers(_jumpableLasers));
            yield return new WaitForSeconds(_data.AttackRange * 0.5f);
            _transform.DOJump(_laserPivot.position, _data.Speed, _data.WalkPointRange, _data.Acceleration * 0.5f)
            .OnComplete(() => ShootLasers(_slideableLasers));
        }
        else
        {
            _transform.DOJump(_laserPivot.position, _data.Speed, _data.WalkPointRange, _data.Acceleration * 0.5f)
            .OnComplete(() => ShootLasers(_slideableLasers));
            yield return new WaitForSeconds(_data.AttackRange * 0.5f);
            _transform.DOJump(_laserPivot.position, _data.Speed, _data.WalkPointRange, _data.Acceleration * 0.5f)
            .OnComplete(() => ShootLasers(_jumpableLasers));
        }
    }

    private void ShootLasers(List<Transform> laserList)
    {
        foreach (Transform laser in laserList)
        {
            laser.gameObject.SetActive(true);
            laser.DOMove(laser.forward * _data.VisionRange, _data.AttackRange)
            .SetRelative(true);
        }
    }

    public void ResetLasers()
    {
        // OJO esto solo funciona para 2 lasers pero no tenia muchas ganas de terminar esto
        // ademas que esta super cutre (crusher)
        int direction = 1;

        foreach (Transform laser in _jumpableLasers)
        {
            laser.gameObject.SetActive(false);
            laser.position = new Vector3(0, laser.position.y, direction);
            direction *= -1;
        }

        direction = 1;
        foreach (Transform laser in _slideableLasers)
        {
            laser.gameObject.SetActive(false);
            laser.position = new Vector3(0, laser.position.y, direction);
            direction *= -1;
        }
    }
    #endregion

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        EventManager.OnUpdateProgressBar(_data.MaxHealth - _currentHp);
    }

    public void Dying()
    {
        EventManager.OnBulletTime(0.5f);

        if (_audio != null)
            _audio.PlaySound((int)SFX.Death);

        _headMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine);

        _mainMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine)
        .OnComplete(() =>
        {
            EventManager.OnResetTime();
            DeathEvent?.Invoke();
        });
    }

    public void CreateExplosion()
    {
        // esto podria ser un componente aparte para no manejarlo en todos los bosses
        GameObject explosion = _explosionPool.GetPooledObject();

        if (!explosion) return;
        Vector3 explosionPos = _headPos.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = explosionPos;
            explosion.SetActive(true);
            explosionScript.HarmlessExplosion(_data.DodgeRange, _data.DodgeSpeed);
        }
    }

    protected override void Death()
    {
        if (_isDead) return;

        _isDead = true;
    }

    private void OnTimerCompleted()
    {
        _canAttack = true;
    }

    protected override void Respawn()
    {
        _currentHp = _data.MaxHealth;
        EventManager.OnDeactivateProgressBar();
        _bossParent.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= Respawn;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    public bool IsMoving
    {
        get { return _isMoving; }
    }

    public bool IsLasering
    {
        get { return _isLasering; }
    }
}
