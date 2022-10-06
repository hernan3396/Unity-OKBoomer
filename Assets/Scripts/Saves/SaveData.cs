using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // player data
    public int WeaponsObtained;
    public Vector3 PlayerPos;
    public int[] Ammo;

    // level data
    public int MaxLevelUnlocked;
    public List<TimerData> TimerInfo = new List<TimerData>() {
        new TimerData("nivel1", 10),
        new TimerData("nivel2", 15),
        new TimerData("nivel3", 20)
    };

    public SaveData(Vector3 playerPos, TimerData timerData)
    {
        PlayerPos = playerPos;
        TimerInfo.Add(timerData);
    }
}

[System.Serializable]
public class TimerData
{
    public string Name;
    public float LevelTime;

    public TimerData(string name, float levelTime)
    {
        Name = name;
        LevelTime = levelTime;
    }
}