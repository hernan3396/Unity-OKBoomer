using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponStateManager : MonoBehaviour
{
    public enum State
    {
        Idle
    }

    [SerializeField] private WeaponBaseState _currentState;
    [SerializeField] private List<WeaponBaseState> _states;
    [SerializeField] private TMP_Text _stateText;

    private void Start()
    {
        _currentState = _states[(int)State.Idle];
        _currentState.OnEnterState(this);

#if UNITY_EDITOR
        _stateText.gameObject.SetActive(true);
#endif
    }

    public virtual void Update()
    {
        _currentState.UpdateState(this);
    }

    public virtual void FixedUpdate()
    {
        _currentState.FixedUpdateState(this);
    }

    public virtual void SwitchState(State state)
    {
        _currentState?.OnExitState(this);

        _stateText.text = "Weapon: " + state.ToString();

        _currentState = _states[(int)state];
        _currentState.OnEnterState(this);
    }
}
