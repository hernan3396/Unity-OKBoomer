using UnityEngine;

public class DiscBullet : Bullet
{
    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        // if (other.transform.TryGetComponent(out Enemy enemy))
        //     enemy.TakeDamage(_damage);

        if (_bounces > 0)
            Bounce();
        else
            DisableBullet();
    }

    private void Bounce()
    {
        _bounces -= 1;
        _rb.velocity = _rb.velocity * 1.1f;
        _bulletTimer = 0;
    }
}