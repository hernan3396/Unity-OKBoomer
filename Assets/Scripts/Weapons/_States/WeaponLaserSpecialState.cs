using UnityEngine;

public class WeaponLaserSpecialState : WeaponBaseState, IPauseable
{
    private Player _player;
    private PlayerShoot _playerShoot;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _laser;
    private LineRenderer _laserLR;

    private WeaponStateManager _state;
    private UtilTimer _utilTimer;
    private bool _isPaused;

    private void Awake()
    {
        EventManager.Pause += OnPause;
    }

    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _state = state;
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
            _laserLR = _laser.GetComponentInChildren<LineRenderer>();

            _utilTimer = GetComponent<UtilTimer>();
        }

        _laser.SetActive(true);

        _utilTimer.StartTimer(_player.CurrentWeaponData.Data.SpecialTime);
        _utilTimer.onTimerCompleted += OnTimerCompleted;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        if (_isPaused) return;

        LaserShoot();

        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void OnTimerCompleted()
    {
        _state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void LaserShoot()
    {
        _laserLR.SetPosition(0, _shootPoint.position);

        if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit, Mathf.Infinity))
        {
            _laserLR.SetPosition(1, hit.point);
            Transform otherTransform = hit.transform;

            if (otherTransform.transform.TryGetComponent(out EnemyHead head))
                head.TakeDamage(_player.CurrentWeaponData.Data.SpecialDamage, otherTransform);

            if (otherTransform.TryGetComponent(out Enemy enemyRB))
                enemyRB.TakeDamage(_player.CurrentWeaponData.Data.SpecialDamage, otherTransform);

            if (otherTransform.TryGetComponent(out Breakable breakable))
                breakable.TakeDamage(_player.CurrentWeaponData.Data.SpecialDamage);

            if (otherTransform.parent == null) return;

            if (otherTransform.parent.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_player.CurrentWeaponData.Data.SpecialDamage, otherTransform);
        }
        else
            _laserLR.SetPosition(1, _shootPoint.position + _shootPoint.forward * 20);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        _laser.SetActive(false);
        _utilTimer.onTimerCompleted -= OnTimerCompleted;
    }

    public void OnPause(bool value)
    {
        _isPaused = value;
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}
