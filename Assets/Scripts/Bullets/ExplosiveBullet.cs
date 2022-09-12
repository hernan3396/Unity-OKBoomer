using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField] private GameObject _bulletModel;
    private PoolManager _explosionPool;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        // llamar explosion
        DisableBullet();

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = _transform.position;
            explosion.SetActive(true);
            explosionScript.StartExplosion(10, 0.5f, _damage);
        }
    }

    public void ActivateBullet()
    {
        gameObject.SetActive(true);
        _transform.parent = null;
        _bulletModel.SetActive(true);
        _rb.useGravity = true;
    }

    protected override void DisableBullet()
    {
        _bulletTimer = 0;
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _bulletModel.SetActive(false);
    }
}
