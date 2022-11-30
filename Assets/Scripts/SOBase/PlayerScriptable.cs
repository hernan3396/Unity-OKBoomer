using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "OKBoomer/New Player Data", order = 0)]
public class PlayerScriptable : ScriptableObject
{
    public int MaxHealth;
    public int Invulnerability;
    public int DeathDuration; // tiene que ser igual a la death anim

    #region Movement
    [Header("Movement")]
    public float CrouchTimer;
    public int CrouchVel;
    public int Speed;
    public float SlideCooldown;
    public LayerMask CeilingLayer;
    #endregion

    #region Jump
    [Header("Jump")]
    public float HalfGravityLimit;
    public float CoyoteMaxTime;
    public float JumpBufferTime;
    public int JumpStrength;
    public int MaxFallSpeed;
    public int Gravity;
    public float MovementModifier;
    #endregion

    #region Look
    [Header("Look")]
    public float MouseSensitivity;
    public Vector2 LookLimits;
    public float SwayMultiplier;
    public float SwaySmoothness;
    public int TiltAngle;
    public int TiltSpeed;
    public Vector2 HurtRecoil;
    public Vector2 HurtShake;
    #endregion

    #region WeaponMovement
    [Header("Weapon Movement")]
    public float WeaponAmplitude;
    public float WeaponFrequency;
    #endregion
}