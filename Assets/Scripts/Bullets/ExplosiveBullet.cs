using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField] private GameObject _bulletModel;
    [SerializeField] private GameObject _explosion;

    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        Explosion();
    }

    private void Explosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _explosion.transform.localScale.x * 0.5f); // ve contra que choca la explosion

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform.parent.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_damage);
        }
        DisableBullet();
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
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _bulletModel.SetActive(false);
        _explosion.SetActive(true);

        Invoke("DisableExplosion", _data.Duration * 0.5f);
    }

    private void DisableExplosion()
    {
        _bulletTimer = 0;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _trailRenderer.Clear();

        _explosion.SetActive(false);
        gameObject.SetActive(false);
    }
}
