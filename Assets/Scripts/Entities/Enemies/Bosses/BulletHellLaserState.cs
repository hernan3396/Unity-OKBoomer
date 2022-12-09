public class BulletHellLaserState : EnemyBaseState
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

        _utilTimer.StartTimer(_enemy.Data.Acceleration * 1.5f);
        _utilTimer.onTimerCompleted += OnTimerCompleted;

        _enemy.ResetLasers();
        _enemy.Lasers();
        EventManager.GameStart += OnGameStart;
    }

    private void OnTimerCompleted()
    {
        // state 1 = attacking
        _state.SwitchState((EnemyStateManager.EnemyState)1);
    }

    private void OnGameStart()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= OnTimerCompleted;

        _state.SwitchState((EnemyStateManager.EnemyState)0);
    }

    public override void UpdateState(EnemyStateManager state)
    {
        // el estado es muerte
        if (_enemy.IsDead) state.SwitchState(EnemyStateManager.EnemyState.Dodging);

        _enemy.RotateTowards(_enemy.Player);
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

        EventManager.GameStart -= OnGameStart;
    }
}
