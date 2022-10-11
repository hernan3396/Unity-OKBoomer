using UnityEngine.AI;

public class EnemyDodgingState : EnemyBaseState
{
    private Enemy _enemy;
    private NavMeshAgent _agent;
    private EnemyStateManager _state;
    private UtilTimer _utilTimer;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _enemy = state.Enemy;
            _agent = _enemy.Agent;
            _state = state;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _agent.speed = _enemy.Data.DodgeSpeed;
        _agent.acceleration = _enemy.Data.DodgeAcceleration;

        _enemy.Dodge();
        _enemy.GoToDestination();
        _enemy.Tookdamage = false;
        _enemy.IsDodging = true;
        _utilTimer.StartTimer(2); // se podria pasar al scriptable pero de mientras...
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(EnemyStateManager state)
    {
        if (_enemy.IsDead) return;

        if (_enemy.DestinationReached())
            state.SwitchState(EnemyStateManager.EnemyState.Attacking);

        // rotar al player y si esta en un rango que dispare
        _enemy.RotateTowards(_enemy.Player);

        if (_enemy.IsLookingAtPlayer())
            _enemy.Attacking();

        if (_enemy.IsPlayerInSight(_enemy.Data.AttackRange)) return;

        state.SwitchState(EnemyStateManager.EnemyState.Chasing);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(EnemyStateManager.EnemyState.Attacking);
    }

    private void OnDestroy()
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _enemy.IsDodging = false;
        _agent.speed = _enemy.Data.Speed;
        _agent.acceleration = _enemy.Data.Acceleration;
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }
}
