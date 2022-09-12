using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{
    public abstract void OnEnterState(EnemyStateManager state);
    public abstract void UpdateState(EnemyStateManager state);
    public abstract void FixedUpdateState(EnemyStateManager state);
    public virtual void OnExitState(EnemyStateManager state) { }
}
