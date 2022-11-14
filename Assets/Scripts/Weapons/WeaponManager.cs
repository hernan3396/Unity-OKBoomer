using UnityEngine;

[RequireComponent(typeof(Player))]
public class WeaponManager : MonoBehaviour
{
    public enum WeaponNumber
    {
        PointingFinger,
        PeepeePiercer,
        TronsEncom
    }

    private Player _player;
    private int _currentWeapon;

    private void Awake()
    {
        EventManager.GameStart += FirstUpdate;
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void FirstUpdate()
    {
        if (_player.MaxWeapons <= 0) return;

        _player.ChangeWeapons(_player.CurrentWeapon);
    }

    public void ChangeWeapon(int side)
    {
        if (_player.MaxWeapons <= 0) return;

        _currentWeapon = _player.CurrentWeapon;
        int maxWeapons = _player.MaxWeapons;

        _currentWeapon += side;

        if (_currentWeapon > maxWeapons - 1)
            _currentWeapon = 0;

        if (_currentWeapon < 0)
            _currentWeapon = maxWeapons - 1;
    }

    public void SetWeapon(int value)
    {
        _currentWeapon = value;
    }

    private void OnDestroy()
    {
        // EventManager.ChangeWeapon -= ChangeWeapon;
        EventManager.GameStart -= FirstUpdate;
    }

    public int CurrentWeapon
    {
        get { return _currentWeapon; }
    }
}
