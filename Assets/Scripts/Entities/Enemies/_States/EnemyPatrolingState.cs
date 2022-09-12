public class EnemyPatrolingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        _enemy.GoToDestination();
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        if (_enemy.Tookdamage)
            state.SwitchState(EnemyStateManager.EnemyState.Chasing);

        if (_enemy.IsPlayerInSight(_enemy.Data.VisionRange))
            state.SwitchState(EnemyStateManager.EnemyState.Chasing);

        if (_enemy.DestinationReached())
            state.SwitchState(EnemyStateManager.EnemyState.Idle);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
