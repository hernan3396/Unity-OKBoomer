using UnityEngine;
using DG.Tweening;

public class SuicidalEnemy : Enemy
{
    private PoolManager _explosionPool;
    [SerializeField] private Transform BodyPivot;

    [SerializeField] private float _floatingHeight = 1;
    [SerializeField] private float _floatingSpeed = 1;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
        FloatingAnim();
    }

    public void FloatingAnim()
    {
        _transform.DOMoveY(_floatingHeight, _floatingSpeed)
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
        _rb.velocity = Vector3.Lerp(_rb.velocity, direction * _data.Speed * 2, Time.deltaTime);
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

    private void Bounce(Collision col)
    {
        Vector3 inNormal;
        Vector3 crashPos;
        Vector3 outDir;

        inNormal = col.contacts[0].normal;
        crashPos = _transform.position;
        outDir = Vector3.Reflect(_rb.velocity, inNormal);

        outDir += new Vector3(0, 10, 0);

        Debug.Log(outDir.normalized);

        _rb.velocity = Vector3.zero;
        _rb.AddForce(outDir.normalized * 100, ForceMode.Impulse);
        // _rb.velocity = outDir.normalized * 20;
    }
}