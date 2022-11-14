public class WallBossDyingState : EnemyBaseState
{
    private WallBoss _enemy;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = (WallBoss)state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _enemy.Dying();
        InvokeRepeating("CreateExplosion", 0, 2);
        _utilTimer.StartTimer(_enemy.Data.DeathDur);
        _utilTimer.onTimerCompleted += CancelExplosion;
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _utilTimer.onTimerCompleted -= CancelExplosion;
    }


    private void CreateExplosion()
    {
        _enemy.CreateExplosion();
    }

    private void CancelExplosion()
    {
        CancelInvoke("CreateExplosion");
    }
}
