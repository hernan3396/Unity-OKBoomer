public class RangedBossIdleState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        state.SwitchState(EnemyStateManager.EnemyState.Chasing);
    }


    public override void UpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
