public class CoreIdleState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = state.Enemy;
        }
    }


    public override void UpdateState(EnemyStateManager state)
    {
        // esto fue porque no sabia como hacerlo mejor con el state manager
        // lo correcto era crear un metodo en el StateManager para manejar esto
        if (!_enemy.IsDead) return;

        // vamos a usar el 01 como estado de muerte, que el enum se llama patroling
        state.SwitchState(EnemyStateManager.EnemyState.Patroling);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
