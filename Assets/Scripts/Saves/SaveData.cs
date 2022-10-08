using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // player data
    public int WeaponsObtained;
    public Vector3 PlayerPos;
    public int PlayerHealth;
    public int[] Ammo;

    public float CheckpointTimer;
    public bool OnALevel;
    public string CurrentLevel;

    // level data
    public List<TimerData> TimerInfo = new List<TimerData>();
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