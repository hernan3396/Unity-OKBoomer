using UnityEngine;
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
    private CapsuleCollider _col;
    private Material _mainMat;
    private Rigidbody _rb;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _data.VisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.AttackRange);
    }
}
