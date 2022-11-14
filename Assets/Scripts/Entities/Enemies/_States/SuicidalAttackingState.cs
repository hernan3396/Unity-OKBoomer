using DG.Tweening;

public class SuicidalAttackingState : EnemyBaseState
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

        _enemy.StopAgent(true); // se frena
        _enemy.RB.isKinematic = false;
        _enemy.Agent.enabled = false;

        _enemy.FloatingTween.Kill();

        _utilTimer.StartTimer(_enemy.Data.AimSpeed);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        // ir hacia el player
        _enemy.Attacking();

        // si el player se va de rango de chase deja de seguirlo
        if (!_enemy.IsPlayerInRange(_enemy.Data.ChasingRange))
            state.SwitchState(EnemyStateManager.EnemyState.Idle);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _enemy.Agent.enabled = true;
        _enemy.StopAgent(false);
        _enemy.RB.isKinematic = true;

        _enemy.FloatingAnim();
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(EnemyStateManager.EnemyState.Dodging);
    }

    private void OnDestroy()
    {
        if (_utilTimer == null) return;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
