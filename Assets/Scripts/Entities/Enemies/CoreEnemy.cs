using UnityEngine.Events;

public class CoreEnemy : Enemy
{
    public UnityEvent DeathEvent;

    public override void Attacking()
    {
        return;
    }

    protected override void Death()
    {
        base.Death();
        DeathEvent?.Invoke();
    }
}
