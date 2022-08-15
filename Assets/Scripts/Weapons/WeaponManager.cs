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

    private void Start()
    {
        _player = GetComponent<Player>();

        _player.ChangeWeapons(_player.CurrentWeapon);
        EventManager.ChangeWeapon += ChangeWeapon;
    }

    private void ChangeWeapon(int side)
    {
        int currentWeapon = _player.CurrentWeapon;
        int maxWeapons = _player.MaxWeapons;

        currentWeapon += side;

        if (currentWeapon > maxWeapons - 1)
            currentWeapon = 0;

        if (currentWeapon < 0)
            currentWeapon = maxWeapons - 1;

        _player.ChangeWeapons(currentWeapon);
    }

    private void OnDestroy()
    {
        EventManager.ChangeWeapon -= ChangeWeapon;
    }
}
