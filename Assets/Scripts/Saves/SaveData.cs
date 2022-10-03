using UnityEngine;

[System.Serializable]
public class SaveData
{
    // player pos data
    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;

    // level data
    public string CurrentLevelName;
    public bool HasData = false;
    public int MaxLevelUnlocked;

    // weapon data
    public int WeaponsObtained;
    public int[] Ammo;

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

[System.Serializable]
public class TimerData
{
    public string Name;
    public float LevelTime;
}