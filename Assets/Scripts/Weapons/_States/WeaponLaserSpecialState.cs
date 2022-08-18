using UnityEngine;

public class WeaponLaserSpecialState : WeaponBaseState
{
    private Player _player;
    private PlayerShoot _playerShoot;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _laser;
    private LineRenderer _laserLR;
    private float _timer;


    public override void OnEnterState(WeaponStateManager state)
    {
        if (_player == null)
        {
            _player = state.Player;
            _playerShoot = _player.PlayerShoot;
            _laserLR = _laser.GetComponentInChildren<LineRenderer>();
        }

        _laser.SetActive(true);
        _timer = 0;
    }

    public override void UpdateState(WeaponStateManager state)
    {
        LaserShoot();

        if (!_playerShoot.IsSpecialShooting)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);

        _timer += Time.deltaTime;

        if (_timer >= _player.CurrentWeaponData.SpecialTime)
            state.SwitchState(WeaponStateManager.State.CooldownSpecial);
    }

    private void LaserShoot()
    {
        _laserLR.SetPosition(0, _shootPoint.position);

        if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit, Mathf.Infinity))
        {
            _laserLR.SetPosition(1, hit.point);

            // if (hit.collider.TryGetComponent(out Enemy enemy))
            //     enemy.TakeDamage(_player.CurrentWeaponData.SpecialDamage, hit.transform);
        }
        else
            _laserLR.SetPosition(1, _shootPoint.position + _shootPoint.forward * 20);
    }

    public override void OnExitState(WeaponStateManager state)
    {
        _laser.SetActive(false);
    }
}
