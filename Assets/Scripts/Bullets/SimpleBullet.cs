
using UnityEngine;

public class SimpleBullet : Bullet
{
    private void Update()
    {
        BulletLifetime();
    }

    protected override void OnHit(Collision other)
    {
        Transform otherTransform = other.transform;

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

        if (otherTransform.parent == null)
        {
            DisableBullet();
            return;
        }

        if (otherTransform.parent.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage(_damage, _transform);

        if (otherTransform.parent.TryGetComponent(out Breakable breakable))
            breakable.TakeDamage(_damage);

        DisableBullet();
    }
}
