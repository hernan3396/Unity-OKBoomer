public class EnemyDodgingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        _enemy.SearchWalkPoint();
        _enemy.GoToDestination();
        _enemy.Tookdamage = false;
        _enemy.IsDodging = true;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        if (_enemy.DestinationReached())
            state.SwitchState(EnemyStateManager.EnemyState.Attacking);

        // rotar al player y si esta en un rango que dispare
        _enemy.RotateTowards(_enemy.Player);

        if (_enemy.IsLookingAtPlayer())
            _enemy.Attacking();

        if (_enemy.IsPlayerInSight(_enemy.Data.AttackRange)) return;

        state.SwitchState(EnemyStateManager.EnemyState.Chasing);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _enemy.IsDodging = false;
    }
}
