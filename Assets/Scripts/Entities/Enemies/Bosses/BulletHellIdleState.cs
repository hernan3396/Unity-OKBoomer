public class BulletHellIdleState : EnemyBaseState
{
    private EnemyStateManager _state;
    private BulletHellBoss _enemy;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _state = state;
            _enemy = (BulletHellBoss)_state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(1);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    private void OnTimerCompleted()
    {
        // state 1 = attacking
        _state.SwitchState((EnemyStateManager.EnemyState)1);
    }

    public override void UpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
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
}
