using UnityEngine;

public class DiscBullet : Bullet
{
    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        Transform otherTransform = other.transform;

        if (_bounces > 0)
            Bounce();
        else
            DisableBullet();

        if (otherTransform.CompareTag("Player"))
        {
            if (_initPosition != Vector3.zero) _player.TakeDamage(_damage, _initPosition);
            else _player.TakeDamage(_damage, _transform.position); // esto aun esta porque el player a veces se pega con sus propias balas, se puede sacar luego

            return;
        }

        // si el enemigo tiene Rigidbody (en este caso el volador)
        // choca contra el gameobject que lo tenga, por eso parece repetido
        // con el primero
        if (otherTransform.TryGetComponent(out Enemy enemyRB))
            enemyRB.TakeDamage(_damage, _transform);

        if (otherTransform.TryGetComponent(out EnemyHead head))
            head.TakeDamage(_damage, _transform);

        if (otherTransform.parent == null) return;

        if (otherTransform.parent.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage(_damage, _transform);

        if (otherTransform.parent.TryGetComponent(out Breakable breakable))
            breakable.TakeDamage(_damage);
    }

    private void Bounce()
    {
        _bounces -= 1;
        _rb.velocity = _rb.velocity * 1.1f;
        _bulletTimer = 0;
    }
}
