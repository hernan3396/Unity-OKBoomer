public class BulletHellIdleState : EnemyBaseState
{
    private BulletHellBoss _enemy;
    private EnemyStateManager _state;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = (BulletHellBoss)state.Enemy;
            _state = state;
        }

        InvokeRepeating("Test", 1, 5);
    }

    public override void UpdateState(EnemyStateManager state)
    {
        _enemy.RotateTowards(_enemy.Player); // para mirar al player
    }

    private void Test()
    {
        // _enemy.ShakeEye();
        _enemy.MoveToWaypoint();
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
