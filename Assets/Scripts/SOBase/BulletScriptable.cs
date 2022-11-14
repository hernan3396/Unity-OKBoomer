using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "OKBoomer/New Bullet", order = 0)]
public class BulletScriptable : ScriptableObject
{
    public enum BulletType
    {
        Simple,
        Disc,
        SMG,
    }

    public float Duration;

    public BulletType AmmoType;
}