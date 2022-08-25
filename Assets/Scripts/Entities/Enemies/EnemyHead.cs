using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public void TakeDamage(int value, Transform other)
    {
        if (_enemy == null) return;

        _enemy.TakeDamage(value * 2, other);
    }
}
