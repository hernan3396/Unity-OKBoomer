using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private PlayAudio _audio;

    private void Awake()
    {
        if (TryGetComponent(out PlayAudio audio))
            _audio = audio;
    }

    public void TakeDamage(int value, Transform other)
    {
        if (_enemy == null) return;

        if (_audio != null)
            _audio.PlaySound();

        _enemy.TakeDamage(value * 2, other);
    }
}
