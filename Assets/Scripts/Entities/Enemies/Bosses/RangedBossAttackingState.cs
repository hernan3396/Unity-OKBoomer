public class RangedBossAttackingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) state.SwitchState(EnemyStateManager.EnemyState.Chasing);

        _enemy.RotateTowards(_enemy.Player);

        if (_enemy.IsLookingAtPlayer())
            _enemy.Attacking();
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
