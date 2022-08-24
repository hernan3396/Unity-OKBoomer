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
        // rotar al player y si esta en un rango que dispare
        _enemy.RotateTowards(_enemy.Player);
        // falta calcular el dotproduct
        // Vector3 lookDir = (_enemy.position - transform.position).normalized;

        //     float lookingForward = Vector3.Dot(transform.forward, lookDir);

        //     if(lookingForward > 0.9f && lookingForward <= 1.1f)

        // aca ataca siguiendo el firerate del arma usar utiltimers 
        // o usar una statemachine basica en rangedenemy.cs
        // y tiene que rotar hacia el player antes de disparar
        if (_enemy.IsPlayerInSight(_enemy.Data.AttackRange)) return;

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
