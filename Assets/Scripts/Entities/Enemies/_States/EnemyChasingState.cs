public class EnemyChasingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        _enemy.Tookdamage = false;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        // ir hacia el player
        _enemy.ChasePlayer();

        // si el player se va de rango de chase deja de seguirlo
        if (!_enemy.IsPlayerInRange(_enemy.Data.ChasingRange))
        {
            state.SwitchState(EnemyStateManager.EnemyState.Idle);
            return; // solo para que no siga al de abajo
        }

        // si el player entra al rango de ataque pasamos a atacar
        if (_enemy.IsPlayerInSight(_enemy.Data.AttackRange))
            state.SwitchState(EnemyStateManager.EnemyState.Attacking);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
