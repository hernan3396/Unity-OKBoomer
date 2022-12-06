using UnityEngine;

public class BulletHellAttackingState : EnemyBaseState
{
    private EnemyStateManager _state;
    private BulletHellBoss _enemy;
    private UtilTimer _utilTimer;
    private bool _canLaser = true;
    // usamos _canLaser para que no spamee el laser 
    // y tenga que disparar al menos 1 vez entre cada laser

    public override void OnEnterState(EnemyStateManager state)
    {
        if (_enemy == null)
        {
            _state = state;
            _enemy = (BulletHellBoss)_state.Enemy;
            _utilTimer = GetComponent<UtilTimer>();
        }

        _utilTimer.StartTimer(1);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
        EventManager.GameStart += OnGameStart;
    }

    private void OnTimerCompleted()
    {
        int attack = Random.Range(0, 2);
        // 0 = shooting, 1 = lasers

        if (!_canLaser) attack = 0;

        // state 2 = shooting
        // state 3 = lasers
        switch (attack)
        {
            case 0:
                _state.SwitchState((EnemyStateManager.EnemyState)2);
                _canLaser = true;
                break;

            case 1:
                _state.SwitchState((EnemyStateManager.EnemyState)3);
                _canLaser = false;
                break;
        }
    }

    private void OnGameStart()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= OnTimerCompleted;

        _state.SwitchState((EnemyStateManager.EnemyState)0);
    }

    public override void UpdateState(EnemyStateManager state)
    {
        // el estado es muerte
        if (_enemy.IsDead) state.SwitchState(EnemyStateManager.EnemyState.Dodging);

        _enemy.RotateTowards(_enemy.Player);
    }

    public override void FixedUpdateState(EnemyStateManager state)
    {
        return;
    }

    public override void OnExitState(EnemyStateManager state)
    {
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    private void OnDestroy()
    {
        if (_utilTimer != null)
            _utilTimer.onTimerCompleted -= OnTimerCompleted;

        EventManager.GameStart -= OnGameStart;
    }
}
