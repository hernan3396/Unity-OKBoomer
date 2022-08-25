using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Enemy))]
public class EnemyStateManager : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patroling,
        Chasing,
        Attacking,
        Dodging
    }

    [SerializeField] private EnemyBaseState _currentState;
    [SerializeField] private List<EnemyBaseState> _states;
    [SerializeField] private TMP_Text _stateText;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _currentState = _states[(int)EnemyState.Idle];
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

    public virtual void SwitchState(EnemyState state)
    {
        _currentState?.OnExitState(this);

        _stateText.text = state.ToString();

        _currentState = _states[(int)state];
        _currentState.OnEnterState(this);
    }

    public Enemy Enemy
    {
        get { return _enemy; }
    }
}
