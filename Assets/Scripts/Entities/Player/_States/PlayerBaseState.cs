using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour
{
    public abstract void OnEnterState(PlayerStateManager stateManager);
    public abstract void UpdateState(PlayerStateManager stateManager);
    public abstract void FixedUpdateState(PlayerStateManager stateManager);
    public virtual void OnExitState(PlayerStateManager stateManager) { }
}
