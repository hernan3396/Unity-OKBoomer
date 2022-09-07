using UnityEngine;

[RequireComponent(typeof(Player))]
public class WeaponManager : MonoBehaviour
{
    /*
    Este script sufrio un cambio en su funcionamiento, antes cambiaba el arma enseguida,
    ahora lo que hace es guardar el valor del arma a cambiar, ya que cuando haces el "Change Out"
    tenes que hacer con la duracion del arma que tenes y luego hacer el cambio del arma, y en el "Change In"
    dura lo del arma cambiada (luego de haber hecho el cambio en "Change Out"), asi que dejo las lineas
    comentadas no por vago, sino por si se rompe algo es solo descomentar eso y sacar en "Change Out" 
    la linea de  "_player.ChangeWeapons(_weaponManager.CurrentWeapon);"
    */
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
        // EventManager.ChangeWeapon += ChangeWeapon;
        EventManager.GameStart += FirstUpdate;
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void FirstUpdate()
    {
        if (_player.GetWeapons.Count == 0) return;

        _player.ChangeWeapons(_player.CurrentWeapon);
    }

    public void ChangeWeapon(int side)
    {
        if (_player.GetWeapons.Count == 0) return;

        _player.GetCurrentModel.GetComponent<Animator>().Play("ChangeOut");

        _currentWeapon = _player.CurrentWeapon;
        int maxWeapons = _player.MaxWeapons;

        _currentWeapon += side;

        if (_currentWeapon > maxWeapons - 1)
            _currentWeapon = 0;

        if (_currentWeapon < 0)
            _currentWeapon = maxWeapons - 1;

        // _player.ChangeWeapons(_currentWeapon);
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
