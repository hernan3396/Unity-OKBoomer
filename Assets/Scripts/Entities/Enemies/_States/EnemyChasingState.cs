public class EnemyChasingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        // ir hacia el player
        _enemy.ChasePlayer();

        // si el player se va de rango de chase deja de seguirlo
        if (!_enemy.IsPlayerInChaseRange())
        {
            state.SwitchState(EnemyStateManager.EnemyState.Idle);
            return; // solo para que no siga al de abajo
        }

        // si el player entra al rango de ataque pasamos a atacar
        if (_enemy.IsPlayerInAttackRange())
            state.SwitchState(EnemyStateManager.EnemyState.Attacking);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
