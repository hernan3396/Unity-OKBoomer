using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] protected Enemy _enemy;
    protected PlayAudio _audio;

    protected void Awake()
    {
        if (TryGetComponent(out PlayAudio audio))
            _audio = audio;
    }

    public virtual void TakeDamage(int value, Transform other)
    {
        if (_enemy == null) return;

        if (_audio != null)
            _audio.PlayOwnSound();

        _enemy.TakeDamage(value * 2, other);
    }
}
