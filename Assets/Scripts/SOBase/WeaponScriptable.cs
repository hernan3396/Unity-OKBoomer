using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "OKBoomer/New Weapon", order = 0)]
public class WeaponScriptable : ScriptableObject
{
    public enum SpecialAttack
    {
        Laser,
        Explosive,
        BulletTime
    }

    public int Id;
    public string Name;

    #region Stats
    [Header("Stats")]
    // mientras mas cercano a (0,0) mas preciso
    public Vector2 Accuracy;
    public float Cooldown;
    public float Startup;
    public int Damage;
    public float ChangeDur;
    #endregion

    #region SpecialShoot
    [Header("Special Shoot")]
    public SpecialAttack SpecialType;
    public int SpecialDamage;
    public float SpecialStartup;
    public float SpecialTime;
    public float SpecialCooldown;
    #endregion

    #region Ammo
    [Header("Ammo")]
    public BulletScriptable.BulletType AmmoType;
    public int MaxBounces;
    public int AmmoSpeed;
    public int MaxAmmo;
    #endregion

    #region Recoil
    [Header("Recoil")]
    public float RecoilForce;
    #endregion

    // esto lo dejo ya por si lo llegamos a usar de esta
    // manera (con el modelo en el scriptable)
    // public GameObject Model;
}