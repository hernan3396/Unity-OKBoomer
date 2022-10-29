using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public abstract class Enemy : Entity, IDamageable, IPauseable
{
    public enum PoolType
    {
        SimpleBullet,
        Blood
    }

    private enum SFX
    {
        Hit,
        Death,
    }

    #region Data
    [Header("Data")]
    [SerializeField] protected EnemyScriptable _data;
    [SerializeField] private bool _respawn = false;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] private MeshRenderer _bodyModel;
    [SerializeField] private SkinnedMeshRenderer _bodyModelSkinned;
    // [SerializeField] private MeshRenderer _headModel;
    [SerializeField] private Transform _headPos;
    protected PoolManager _bloodPool;
    protected CapsuleCollider _col;
    // protected SphereCollider
    protected Material _mainMat;
    protected Material _headMat;
    protected PlayAudio _audio;
    protected Rigidbody _rb;
    #endregion

    #region AI
    [Header("AI")]
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _visionLayer;
    protected Vector3 _destination;
    protected NavMeshAgent _agent;
    protected Transform _player;
    protected PlayerMovement _playerMov;
    protected bool _canAttack = true;
    protected bool _tookDamage = false;
    protected bool _isDodging = false;
    private Vector2 _playerInputLerped;
    private Vector3 _initPos;
    protected Vector3 _lookDir;
    #endregion

    #region States
    protected bool _isPaused = false;
    protected bool _isDead = false;
    protected Vector3 _lastVel;
    #endregion

    private void Awake()
    {
        SetComponents();

        _currentHp = _data.MaxHealth;
        _initPos = _transform.position;

        if (_respawn)
            EventManager.GameStart += Respawn;
    }

    protected virtual void SetComponents()
    {
        if (_bodyModel == null)
            _mainMat = _bodyModelSkinned.material;
        else
            _mainMat = _bodyModel.materials[0];

        _headMat = _headPos.gameObject.GetComponent<MeshRenderer>().material;
        _transform = GetComponent<Transform>();
        _col = GetComponentInChildren<CapsuleCollider>();

        if (TryGetComponent(out PlayAudio audio))
            _audio = audio;


        if (TryGetComponent(out Rigidbody rb))
            _rb = rb;

        if (TryGetComponent(out NavMeshAgent agent))
        {
            _agent = agent;
            _agent.acceleration = _data.Acceleration;
            _agent.speed = _data.Speed;
        }
    }

    protected virtual void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Transform>();
        _playerMov = _player.GetComponent<Player>().PlayerMovement;
        _bloodPool = GameManager.GetInstance.GetEnemyPools[(int)PoolType.Blood];
    }

    #region DamageMethods
    public void TakeDamage(int value, Transform bullet)
    {
        // esta solo crea las particulas y luego llama al
        // takedamage de base
        if (_isInmune || _isDead) return;

        if (!_isDodging)
            _tookDamage = true;

        GameObject blood = _bloodPool.GetPooledObject();
        if (blood)
        {
            blood.transform.position = bullet.position;
            blood.transform.forward = bullet.forward;
            blood.SetActive(true);
        }

        if (_audio != null)
            _audio.PlayOwnSound((int)SFX.Hit);

        // en el codigo de las particulas de la sangre
        // ya esta puesto play on awake y disable en stop action

        TakeDamage(value);
    }

    protected override void Death()
    {
        _isDead = true;
        _col.enabled = false;

        EventManager.OnWaveUpdated();

        if (_audio != null)
            _audio.PlaySound((int)SFX.Death);

        _headMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.OutQuint);

        _mainMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => gameObject.SetActive(false));
    }

    protected virtual void Respawn()
    {
        if (_col == null) return;

        _currentHp = _data.MaxHealth;
        _isDead = false;
        _col.enabled = true;
        _transform.position = _initPos;
        _headMat.SetFloat("_DissolveValue", 0);
        _mainMat.SetFloat("_DissolveValue", 0);
        gameObject.SetActive(true);
    }
    #endregion

    #region Pause
    public void OnPause(bool value)
    {
        _isPaused = value;

        if (_isPaused)
            PauseEnemy();
        else
            ResumeEnemy();
    }

    protected virtual void PauseEnemy()
    {
        if (_rb == null) return;

        _lastVel = _rb.velocity;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
    }
    protected virtual void ResumeEnemy()
    {
        if (_rb == null) return;

        _rb.velocity = _lastVel;
        _rb.useGravity = true;
    }
    #endregion

    #region SightMethods
    public bool IsPlayerInSight(float range)
    {
        Collider[] circleHit = Physics.OverlapSphere(_transform.position, range, _playerLayer);

        if (circleHit.Length > 0 && circleHit[0].CompareTag("Player"))
        {
            Vector3 playerPos = circleHit[0].transform.position;

            return Utils.RayHit(_headPos.position, playerPos, "Player", range, _visionLayer);
        }

        return false;
    }

    public bool IsPlayerInRange(float range)
    {
        float playerDistance = Utils.CalculateDistance(_transform.position, _player.position);
        return playerDistance < range;
    }

    public bool IsLookingAtPlayer()
    {
        Vector3 lookDir = (_player.position - _transform.position).normalized;
        float lookingForward = Vector3.Dot(_transform.forward, lookDir);

        if (lookingForward > 0.8f && lookingForward <= 1.2f)
            return true;

        return false;
    }
    #endregion

    #region MovementMethods
    public void SearchWalkPoint()
    {
        //Calculates random point in range
        Vector3 randDir = Utils.RandomNavSphere(_transform.position, _data.WalkPointRange);
        _destination = randDir;
    }

    public void Dodge()
    {
        int dir = Random.Range(-1, 1);
        if (dir == 0) dir = 1;

        Vector3 predictedDir = _transform.right * dir * _data.DodgeRange;
        Vector3 dodgeDir = _transform.position + predictedDir;

        if (Utils.IsPointInNavMesh(dodgeDir, 4))
            _destination = dodgeDir;
        else
            _destination = _transform.position;
    }

    public void GoToDestination()
    {
        _agent.SetDestination(_destination);
    }

    public bool DestinationReached()
    {
        return Utils.CalculateDistanceNoHeight(_transform.position, _destination) < 2;
    }

    // se podrian juntar estos dos de abajo
    // pero creo que separados tienen sentido
    // si se empiezan a agregar juntarlos en uno solo

    public void ChaseDirection(Vector3 dir)
    {
        _agent.SetDestination(dir);
    }

    public void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    protected Vector3 PredictMovement()
    {
        float timeToReach = Vector3.Distance(_transform.position, _player.position) / _data.Weapon.AmmoSpeed;
        _playerInputLerped = Vector2.Lerp(_playerInputLerped, _playerMov.DirInput, Time.deltaTime * _data.AimSpeed);
        Vector3 forwSpeed = _playerMov.GetVelocity.magnitude * _playerInputLerped;

        int dir = 1;

        if (_transform.right.x < 0)
            dir = -1;

        forwSpeed *= -dir;

        Vector3 result = forwSpeed * timeToReach;
        result.y = 0;

        return result;
    }

    public void RotateTowards(Transform other)
    {
        _lookDir = Utils.CalculateDirection(_transform.position, _player.position + PredictMovement());
        _transform.rotation = Quaternion.LookRotation(_lookDir);
        // _transform.forward = Vector3.Slerp(_transform.forward, lookDir, Time.deltaTime * _data.AimSpeed);
    }

    public void StopAgent(bool value)
    {
        _agent.isStopped = value;
    }
    #endregion

    #region Attacking
    public abstract void Attacking();
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _data.VisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _data.ChasingRange);
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= Respawn;
    }

    public EnemyScriptable Data
    {
        get { return _data; }
    }

    public Transform HeadPos
    {
        get { return _headPos; }
    }

    public NavMeshAgent Agent
    {
        get { return _agent; }
    }

    public Rigidbody RB
    {
        get { return _rb; }
    }

    public Transform Player
    {
        get { return _player; }
    }

    public bool IsDead
    {
        get { return _isDead; }
    }

    public bool Tookdamage
    {
        // se usa mas que nada por si le pegas fuera del rango de chasing
        get { return _tookDamage; }
        set { _tookDamage = value; }
    }

    public bool IsDodging
    {
        get { return _isDodging; }
        set { _isDodging = value; }
    }
}
