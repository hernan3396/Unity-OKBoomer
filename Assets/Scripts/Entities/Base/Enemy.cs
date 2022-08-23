using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public abstract class Enemy : Entity, IDamageable, IPauseable
{
    #region Data
    [Header("Data")]
    [SerializeField] protected EnemyScriptable _data;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] private Transform _headPos;
    protected PoolManager _bulletsPool;
    protected PoolManager _bloodPool;
    protected CapsuleCollider _col;
    protected Material _mainMat;
    protected Rigidbody _rb;
    #endregion

    #region AI
    [Header("AI")]
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected LayerMask _groundLayer;
    protected Vector3 _destination;
    protected NavMeshAgent _agent;
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
    public bool IsPlayerInSight()
    {
        // se usa para detectar al player
        Collider[] circleHit = Physics.OverlapSphere(_transform.position, _data.VisionRange, _playerLayer);

        if (circleHit.Length > 0 && circleHit[0].CompareTag("Player"))
        {
            Vector3 playerPos = circleHit[0].transform.position;

            Debug.DrawRay(_headPos.position, playerPos - _headPos.position, Color.blue);
            // lanzamos un rayo a ver si pega contra el
            bool playerOnSight = Physics.Raycast(_headPos.position, playerPos - _headPos.position, _data.VisionRange);
            return playerOnSight;
        }

        return false;
    }

    // estos dos de abajo se pueden hacer en uno pero
    // asi quedan mas descriptivos
    // si agregamos mas si pasar el valor que queremos
    // chequear por parametro 

    public bool IsPlayerInAttackRange()
    {
        float playerDistance = Utils.CalculateDistance(_transform.position, _player.position);
        return playerDistance < _data.AttackRange;
    }

    public bool IsPlayerInChaseRange()
    {
        // se usa para saber si seguir chaseando al player
        float playerDistance = Utils.CalculateDistance(_transform.position, _player.position);
        return playerDistance < _data.ChasingRange;
    }

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
        return Utils.CalculateDistance(_transform.position, _destination) < 2;
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

    public void UseAgent(bool value)
    {
        _agent.isStopped = value;
    }
    #endregion

    #region Attacking
    protected abstract void Attacking();
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
}
