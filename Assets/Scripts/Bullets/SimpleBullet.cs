using UnityEngine;

public class SimpleBullet : Bullet
{
    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        // if (other.transform.TryGetComponent(out Enemy enemy))
        //     enemy.TakeDamage(_damage, transform);

        DisableBullet();
    }
}
