using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "OKBoomer/New Enemy", order = 0)]
public class EnemyScriptable : ScriptableObject
{
    public int MaxHealth;
    public float DeathDur;
    public float Invulnerability;
    public WeaponScriptable Weapon;

    #region Movement
    [Header("Movement")]
    public int Speed;
    public int Acceleration;
    public int WalkPointRange;
    #endregion

    #region Dodge
    [Header("Dodge")]
    public int DodgeRange;
    public int DodgeSpeed;
    public int DodgeAcceleration;
    #endregion

    #region Vision
    [Header("Vision")]
    public int VisionRange;
    public int AttackRange;
    public int ChasingRange;
    public int AimSpeed;
    #endregion
}
