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

    #region Data
    [Header("Data")]
    [SerializeField] protected EnemyScriptable _data;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] private MeshRenderer _bodyModel;
    [SerializeField] private MeshRenderer _headModel;
    [SerializeField] private Transform _headPos;
    protected PoolManager _bloodPool;
    protected CapsuleCollider _col;
    // protected SphereCollider
    protected Material _mainMat;
    protected Material _headMat;
    protected Rigidbody _rb;
    #endregion

    #region AI
    [Header("AI")]
    [SerializeField] protected LayerMask _playerLayer;
    protected Vector3 _destination;
    protected NavMeshAgent _agent;
    protected Transform _player;
    protected bool _canAttack = true;
    protected bool _tookDamage = false;
    protected bool _isDodging = false;
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
    }

    protected virtual void SetComponents()
    {
        _mainMat = _bodyModel.materials[0];
        _headMat = _headModel.materials[0];
        _transform = GetComponent<Transform>();
        _col = GetComponentInChildren<CapsuleCollider>();


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

        // en el codigo de las particulas de la sangre
        // ya esta puesto play on awake y disable en stop action

        TakeDamage(value);
    }

    protected override void Death()
    {
        _isDead = true;
        _col.enabled = false;

        _headMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.OutQuint);

        _mainMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => gameObject.SetActive(false));
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

            return Utils.RayHit(_headPos.position, playerPos, "Player", range);
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

        if (lookingForward > 0.9f && lookingForward <= 1.1f)
            return true;

        return false;
    }
    #endregion

    #region MovementMethods
    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-_data.WalkPointRange, _data.WalkPointRange);
        float randomX = Random.Range(-_data.WalkPointRange, _data.WalkPointRange);

        _destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
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

    public void RotateTowards(Transform other)
    {
        Vector3 lookDir = Utils.CalculateDirection(_transform.position, _player.position);

        Vector3 newDir = Vector3.RotateTowards(_transform.forward, lookDir, _data.Speed * Time.deltaTime, 0.0f);
        _transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void StopAgent(bool value)
    {
        _agent.isStopped = value;
    }
    #endregion

    #region Attacking
    public abstract void Attacking();
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _data.VisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _data.ChasingRange);
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
