public class WallBossAttackingState : EnemyBaseState
{
    private WallBoss _enemy;
    private EnemyStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = (WallBoss)state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_enemy.LaserTime);
        _utilTimer.onTimerCompleted += ShootLasers;
    }

    private void ShootLasers()
    {
        _enemy.StartLaser();
        _utilTimer.StartTimer(_enemy.LaserTime);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) state.SwitchState(EnemyStateManager.EnemyState.Chasing);
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _utilTimer.onTimerCompleted -= ShootLasers;
    }

    private void OnDestroy()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= ShootLasers;
    }
}
