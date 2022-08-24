using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private Enemy _enemy;

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
            _enemy = state.Enemy;

        _enemy.UseAgent(true); // se frena
    }

    public override void UpdateState(EnemyStateManager state)
    {
        // aca ataca siguiendo el firerate del arma usar utiltimers 
        // o usar una statemachine basica en rangedenemy.cs
        // y tiene que rotar hacia el player antes de disparar
        if (_enemy.IsPlayerInAttackRange()) return;

        state.SwitchState(EnemyStateManager.EnemyState.Chasing);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _enemy.UseAgent(false);
    }
}
