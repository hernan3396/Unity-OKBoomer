public class SuicidalExplodingState : EnemyBaseState
{
    private EnemyStateManager _state;
    private SuicidalEnemy _enemy;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _state = state;
            _enemy = (SuicidalEnemy)_state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(_enemy.Data.DodgeRange);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    private void OnTimerCompleted()
    {
        _enemy.CallExplosion();
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
        if (_utilTimer == null) return;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
