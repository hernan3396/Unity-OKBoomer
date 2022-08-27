
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
        {
            if (_initPosition != Vector3.zero) _player.TakeDamage(_damage, _initPosition);
            else _player.TakeDamage(_damage, _transform.position); // esto aun esta porque el player a veces se pega con sus propias balas, se puede sacar luego
        }

        if (other.transform.TryGetComponent(out EnemyHead head))
            head.TakeDamage(_damage, _transform);

        DisableBullet();
    }
}
