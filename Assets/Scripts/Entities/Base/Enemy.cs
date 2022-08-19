using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : Entity, IDamageable, IPauseable
{
    #region Data
    [Header("Data")]
    [SerializeField] protected EnemyScriptable _data;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] protected PoolManager _bulletsPool;
    [SerializeField] protected PoolManager _bloodPool;
    protected CapsuleCollider _col;
    protected Material _mainMat;
    protected Rigidbody _rb;
    #endregion

    #region AI
    [Header("AI")]
    [SerializeField] protected LayerMask _groundLayer;
    protected NavMeshAgent _agent;
    protected bool _playerInRange;
    protected Transform _player;
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
        _mainMat = GetComponentInChildren<MeshRenderer>().materials[0];
        _transform = GetComponent<Transform>();
        _col = GetComponentInChildren<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();

        if (TryGetComponent(out NavMeshAgent agent))
            _agent = agent;
    }

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Transform>();
    }

    #region DamageMethods
    public void TakeDamage(int value, Transform bullet)
    {
        // esta solo crea las particulas y luego llama al
        // takedamage de base
        if (_isInmune || _isDead) return;

        GameObject blood = _bloodPool.GetPooledObject();
        if (!blood) return;

        blood.transform.position = bullet.position;
        blood.transform.forward = bullet.forward;

        blood.SetActive(true);
        // en el codigo de las particulas de la sangre
        // ya esta puesto play on awake y disable en stop action

        TakeDamage(value);
    }

    protected override void Death()
    {
        _isDead = true;
        _col.enabled = false;

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
        _lastVel = _rb.velocity;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
    }
    protected virtual void ResumeEnemy()
    {
        _rb.velocity = _lastVel;
        _rb.useGravity = true;
    }
    #endregion

    #region MovementMethods
    public void IsPlayerInRange()
    {
        float playerDistance = Utils.CalculateDistance(_transform.position, _player.position).magnitude;
        _playerInRange = playerDistance < _data.VisionRange;
    }

    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-_data.WalkPointRange, _data.WalkPointRange);
        float randomX = Random.Range(-_data.WalkPointRange, _data.WalkPointRange);

        Vector3 _destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_destination, -transform.up, 2f, _groundLayer))
            _agent.SetDestination(_destination);
    }

    public void ChaseDirection(Vector3 dir)
    {
        _agent.SetDestination(dir);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _data.VisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);
    }

    public bool PlayerInRange
    {
        get { return _playerInRange; }
    }
}
