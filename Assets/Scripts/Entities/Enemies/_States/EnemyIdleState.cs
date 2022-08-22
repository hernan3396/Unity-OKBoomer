public class EnemyIdleState : EnemyBaseState
{
    private Enemy _enemy;

    private EnemyStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _state = state;
            _enemy = _state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _enemy.SearchWalkPoint();

        _utilTimer.StartTimer(2); // se podria pasar al scriptable pero de mientras...
        _utilTimer.onTimerCompleted += OnTimerCompleted;
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

    private void OnTimerCompleted()
    {
        _state.SwitchState(EnemyStateManager.EnemyState.Patroling);
    }
}
