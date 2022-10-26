using UnityEngine;
using DG.Tweening;

public class SuicidalEnemy : Enemy
{
    private PoolManager _explosionPool;
    [SerializeField] private Transform BodyPivot;

    [SerializeField] private float _floatingHeight = 1;
    [SerializeField] private float _floatingSpeed = 1;

    private Tween _floatingTween;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
        FloatingAnim();
    }

    public void FloatingAnim()
    {
        _floatingTween = _transform.DOMoveY(_floatingHeight, _floatingSpeed)
          .SetLoops(-1, LoopType.Yoyo)
          .SetEase(Ease.Linear)
          .SetRelative(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        Transform otherTransform = other.transform;

        if (otherTransform.CompareTag("Bullet"))
            Death();

        if (otherTransform.CompareTag("Player"))
            Death();

        Bounce(other);
    }

    public override void Attacking()
    {
        Vector3 direction = Utils.CalculateDirection(_transform.position, _player.position);
        _rb.velocity = Vector3.Lerp(_rb.velocity, direction * _data.Speed * 2, Time.deltaTime * _data.DodgeAcceleration);
    }

    protected override void Death()
    {
        base.Death();

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = _transform.position;
            explosion.SetActive(true);
            explosionScript.StartExplosion(_data.Weapon.Startup, 0.5f, _data.Weapon.Damage);
        }
    }

    public void CallExplosion()
    {
        Death();
    }

    protected override void Respawn()
    {
        base.Respawn();
    }

    private void Bounce(Collision col)
    {
        Vector3 inNormal;
        Vector3 crashVel;

        Vector3 outDir;

        inNormal = col.contacts[0].normal;
        crashVel = (_rb.velocity).normalized; // la direccion opuesta

        outDir = Vector3.Reflect(crashVel, inNormal);
        outDir.y = 1f;

        _rb.velocity = Vector3.zero;
        _rb.AddForce(outDir.normalized * _data.DodgeSpeed, ForceMode.Impulse);
    }

    public Tween FloatingTween
    {
        get { return _floatingTween; }
    }
}