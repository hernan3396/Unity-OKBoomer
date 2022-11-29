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

        InvokeRepeating("Test", 1, 2);
        // Invoke("Test", 1);
    }

    public override void UpdateState(EnemyStateManager state)
    {
        _enemy.RotateTowards(_enemy.Player); // para mirar al player
    }

    private void Test()
    {
        _enemy.LaserInitPos();
        // _enemy.ShakeEye();
        // _enemy.MoveToWaypoint();
        _enemy.Shoot();
        // _enemy.Lasers();
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }
}
