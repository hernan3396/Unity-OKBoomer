using UnityEngine;

public class TrackerBullet : Bullet
{
    [SerializeField] private Transform _model;

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
}
