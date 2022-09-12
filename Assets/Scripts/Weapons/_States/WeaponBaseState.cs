using UnityEngine;

public abstract class WeaponBaseState : MonoBehaviour
{
    public abstract void OnEnterState(WeaponStateManager state);
    public abstract void UpdateState(WeaponStateManager state);
    public virtual void OnExitState(WeaponStateManager state) { }
}
