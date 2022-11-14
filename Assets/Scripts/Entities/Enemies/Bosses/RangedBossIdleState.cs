public class RangedBossIdleState : EnemyBaseState
{
    private Enemy _enemy;
    private EnemyStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
            _state = state;
        }

        _utilTimer.StartTimer(_enemy.Data.ChasingRange);
        _utilTimer.onTimerCompleted += Attack;
    }

    private void Attack()
    {
        _state.SwitchState(EnemyStateManager.EnemyState.Patroling); // attacking pero el enum es el 01
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
        _utilTimer.onTimerCompleted -= Attack;
    }

    private void OnDestroy()
    {
        _utilTimer.onTimerCompleted -= Attack;
    }
}
