using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "OKBoomer/New Player Data", order = 0)]
public class PlayerScriptable : ScriptableObject
{
    public int MaxHealth;
    public int Invulnerability;

    #region Movement
    [Header("Movement")]
    public float CrouchTimer;
    public int CrouchVel;
    public int Speed;
    #endregion

    #region Jump
    [Header("Jump")]
    public float HalfGravityLimit;
    public float CoyoteMaxTime;
    public int JumpStrength;
    public int MaxFallSpeed;
    public int Gravity;
    #endregion

    #region Look
    [Header("Look")]
    public float MouseSensitivity;
    public Vector2 LookLimits;
    public float SwayMultiplier;
    public float SwaySmoothness;
    #endregion
}