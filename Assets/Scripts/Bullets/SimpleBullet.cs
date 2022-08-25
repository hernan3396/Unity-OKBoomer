
using UnityEngine;

public class SimpleBullet : Bullet
{
    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        if (other.transform.parent.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage(_damage, _transform);

        if (other.transform.CompareTag("Player"))
            _player.TakeDamage(_damage);

        if (other.transform.TryGetComponent(out EnemyHead head))
            head.TakeDamage(_damage, _transform);

        DisableBullet();
    }
}
