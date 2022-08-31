public class StaticIdleState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        if (_enemy.IsPlayerInSight(_enemy.Data.AttackRange))
            state.SwitchState(EnemyStateManager.EnemyState.Patroling);
        // pasa a patroling por el ENUM pero en realidad esta pasando al attacking
        // podria decir que pase al attacking pero el array de los states quedaria con
        // espacios vacios
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
