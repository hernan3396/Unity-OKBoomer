using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;

    public bool HasData = false;

    public int WeaponsObtained;
    public int[] Ammo;

    public int MaxLevelUnlocked;

    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPosX = position.x;
        PlayerPosY = position.y;
        PlayerPosZ = position.z;
    }

    public Vector3 GetPlayerPosition()
    {
        return new Vector3(PlayerPosX, PlayerPosY, PlayerPosZ);
    }
}
