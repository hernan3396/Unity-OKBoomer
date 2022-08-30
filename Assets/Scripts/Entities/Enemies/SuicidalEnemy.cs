using UnityEngine;

public class SuicidalEnemy : Enemy
{
    public override void Attacking()
    {
        Vector3 direction = Utils.CalculateDirection(_transform.position, _player.position);
        _rb.velocity = Vector3.Lerp(_rb.velocity, direction * _data.Speed * 2, Time.deltaTime);
    }
}