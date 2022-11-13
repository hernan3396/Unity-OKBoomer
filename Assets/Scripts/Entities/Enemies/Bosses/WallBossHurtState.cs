public class WallBossHurtState : EnemyBaseState
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

        InvokeRepeating("ShootLasers", 0, 1);
        _utilTimer.StartTimer(2);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(EnemyStateManager.EnemyState.Patroling);
    }

    private void ShootLasers()
    {
        // _enemy.Hurt();
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    private void OnDestroy()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= OnTimerCompleted;
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
