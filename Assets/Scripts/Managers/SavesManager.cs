using UnityEngine;
using System;

public class SavesManager : MonoBehaviour
{
    [SerializeField] private string _savesPrefix = "Hyperwave_";
    [SerializeField] private SavesData[] _timers;
    [SerializeField] private int _levelNumber;


    private void Awake()
    {
        EventManager.SaveTime += SaveTime;

        LoadTimers();
    }

    private void LoadTimers()
    {
        foreach (SavesData item in _timers)
        {
            string prefName = _savesPrefix + item.Name;

            if (PlayerPrefs.HasKey(prefName + "_string"))
            {
                item.valueString = PlayerPrefs.GetString(prefName + "_string");
                item.valueInt = PlayerPrefs.GetFloat(prefName + "_float");
            }
        }
    }

    private void Start()
    {
        EventManager.OnLoadTimer(_timers);
    }

    private void SaveTime(string valueString, float valueInt)
    {
        SavesData timer = _timers[_levelNumber];
        if (valueInt >= timer.valueInt) return; // solo guardamos el menor tiempo

        timer.valueString = valueString;
        timer.valueInt = valueInt;

        string prefName = _savesPrefix + timer.Name;
        PlayerPrefs.SetString(prefName + "_string", valueString);
        PlayerPrefs.SetFloat(prefName + "_float", valueInt);
    }
}

[Serializable]
public class SavesData
{
    public string Name;
    public string valueString;
    public float valueInt;
}