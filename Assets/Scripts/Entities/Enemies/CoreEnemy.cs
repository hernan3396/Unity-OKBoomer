using UnityEngine;
using UnityEngine.Events;

public class CoreEnemy : Enemy
{
    public UnityEvent DeathEvent;
    [SerializeField] private float _intensityStart = 0.15f;

    public override void Attacking()
    {
        return;
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        float intensityValue = (_data.MaxHealth - _currentHp) * 0.01f;

        _mainMat.SetFloat("_Intensity", _intensityStart + intensityValue);
    }

    protected override void Death()
    {
        base.Death();
        DeathEvent?.Invoke();
    }
}
