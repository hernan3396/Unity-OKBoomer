public class WallBossAttackingState : EnemyBaseState
{
    private WallBoss _enemy;
    private EnemyStateManager _state;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = (WallBoss)state.Enemy;

        _enemy.Attacking();
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        return;
    }
}
