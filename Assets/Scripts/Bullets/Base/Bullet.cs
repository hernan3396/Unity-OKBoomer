using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    #region Components
    protected TrailRenderer _trailRenderer;
    protected Transform _transform;
    protected Rigidbody _rb;
    protected Player _player;
    #endregion

    #region Data
    [SerializeField] protected BulletScriptable _data;
    protected float _bulletTimer;

    // estos ultimos 3 vienen del arma
    protected int _damage;
    protected int _speed;
    protected int _bounces;
    #endregion

    #region Pause
    protected Vector3 _lastAngVel; // algunas tienen velocidad angular
    protected Vector3 _lastVel;
    protected bool _isPaused;
    #endregion

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();
        EventManager.Pause += OnPause;
    }

    public virtual void Shoot(Vector2 acc)
    {
        // los dividimos entre 100 porque asi quedan numeros mas
        // lindos en el scriptable del arma
        float hAcc = acc.x * 0.01f;
        float vAcc = acc.y * 0.01f;

        // aca falta normalizar algun valor

        Vector3 dir = new Vector3(Random.Range(-hAcc, hAcc), Random.Range(-vAcc, vAcc), Random.Range(-hAcc, hAcc));
        dir = dir + _transform.forward;

        _rb.AddForce(dir * _speed, ForceMode.Impulse);
    }

    public void SetData(int damage, int speed, int bounces, Transform transform)
    {
        _transform.position = transform.position;
        _transform.rotation = transform.rotation;

        // ajustes de estadisticas
        _damage = damage;
        _speed = speed;
        _bounces = bounces;
    }

    protected virtual void BulletLifetime()
    {
        if (_isPaused) return;
        _bulletTimer += Time.deltaTime;

        if (_bulletTimer >= _data.Duration)
            DisableBullet();
    }

    protected abstract void OnHit(Collision other);

    public void OnPause(bool value)
    {
        _isPaused = value;

        if (_isPaused)
            PauseBullet();
        else
            ResumeBullet();
    }

    private void PauseBullet()
    {
        _lastAngVel = _rb.angularVelocity;
        _lastVel = _rb.velocity;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
    }

    private void ResumeBullet()
    {
        _rb.angularVelocity = _lastAngVel;
        _rb.velocity = _lastVel;
        _rb.useGravity = true;
    }

    protected virtual void DisableBullet()
    {
        // resetea parametros
        _bulletTimer = 0;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _trailRenderer.Clear();

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        OnHit(other);
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}
