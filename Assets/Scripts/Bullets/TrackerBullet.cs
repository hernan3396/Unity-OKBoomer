using UnityEngine;

public class TrackerBullet : Bullet
{
    [SerializeField] private Transform _model;
    private PoolManager _explosionPool;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    private void Update()
    {
        BulletLifetime();
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        Vector3 dir = Utils.CalculateDirection(_transform.position, _player.Transform.position);
        _rb.velocity = Vector3.Lerp(_rb.velocity, dir * _speed, Time.deltaTime * _bounces);
        _model.forward = dir;
        // use bounces como la "Aceleracion"
    }

    protected override void OnHit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (_initPosition != Vector3.zero) _player.TakeDamage(_damage, _initPosition);
            else _player.TakeDamage(_damage, _transform.position);
        }

        DisableBullet();
    }

    protected override void DisableBullet()
    {
        base.DisableBullet();

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = _transform.position;
            explosion.SetActive(true);
            explosionScript.StartExplosion(10, 0.5f, _damage);
        }
    }
}
