using UnityEngine;

public class SuicidalAttackingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        _enemy.StopAgent(true); // se frena
        _enemy.RB.isKinematic = false;
        _enemy.Agent.enabled = false;
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
    }
}
