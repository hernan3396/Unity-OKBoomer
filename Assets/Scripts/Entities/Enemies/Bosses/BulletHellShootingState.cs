public class BulletHellShootingState : EnemyBaseState
{
    private EnemyStateManager _state;
    private BulletHellBoss _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _state = state;
            _enemy = (BulletHellBoss)_state.Enemy;
        }

        _enemy.MoveToWaypoint();
        EventManager.GameStart += OnGameStart;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        // el estado es muerte
        if (_enemy.IsDead) state.SwitchState(EnemyStateManager.EnemyState.Dodging);

        _enemy.StartShooting();
        _enemy.RotateTowards(_enemy.Player);

        if (_enemy.IsMoving) return;
        state.SwitchState((EnemyStateManager.EnemyState)1);
    }

    private void OnGameStart()
    {
        _state.SwitchState((EnemyStateManager.EnemyState)0);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= OnGameStart;
    }
}
