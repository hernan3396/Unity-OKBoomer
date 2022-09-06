using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SavesManager : MonoBehaviour
{
    [SerializeField] private string _savesPrefix = "Hyperwave_";
    [SerializeField] private SavesData[] _timers;
    private string _levelName;


    private void Start()
    {
        EventManager.SaveTime += SaveTime;

        _levelName = SceneManager.GetActiveScene().name;
        LoadTimers();
    }

    private void LoadTimers()
    {
        if (_timers.Length <= 0) return;

        foreach (SavesData item in _timers)
        {
            string prefName = _savesPrefix + item.Name; // same as _levelName

            if (PlayerPrefs.HasKey(prefName + "_string"))
            {
                item.valueString = PlayerPrefs.GetString(prefName + "_string");
                item.valueInt = PlayerPrefs.GetFloat(prefName + "_float");
            }
        }
        EventManager.OnLoadTimer(_timers);
    }

    private void SaveTime(string valueString, float valueInt)
    {
        string prefName = _savesPrefix + _levelName;

        if (PlayerPrefs.HasKey(prefName + "_string") &&
         valueInt >= PlayerPrefs.GetFloat(prefName + "_float"))
            return; // solo guardamos el menor tiempo

        PlayerPrefs.SetString(prefName + "_string", valueString);
        PlayerPrefs.SetFloat(prefName + "_float", valueInt);
    }

    private void OnDestroy()
    {
        EventManager.SaveTime -= SaveTime;
    }
}

[Serializable]
public class SavesData
{
    public string Name;
    public string valueString;
    public float valueInt;
}