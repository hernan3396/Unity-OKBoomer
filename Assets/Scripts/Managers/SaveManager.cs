using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    private Player _player;
    private FileStream _fileStream;
    private string _fileName = "data";

    private void Awake()
    {
        _fileName = Application.persistentDataPath + "/" + _fileName + ".save";
        CreateFile();
    }

    private void CreateFile()
    {
        if (!File.Exists(_fileName))
        {
            _fileStream = File.Create(_fileName);
            _fileStream.Close();
            return;
        }
    }

    public void GameSave(SaveData save)
    {
        string json = JsonUtility.ToJson(save);

        BinaryFormatter bf = new BinaryFormatter();
        _fileStream = File.Open(_fileName, FileMode.Open);
        bf.Serialize(_fileStream, json);
        _fileStream.Close();
    }

    public SaveData GameLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();

        _fileStream = File.Open(_fileName, FileMode.Open);

        if (_fileStream.Length == 0)
        {
            _fileStream.Close();
            return CreateEmptyData();
        }

        string save = (string)bf.Deserialize(_fileStream);
        _fileStream.Close();

        SaveData loadedData = JsonUtility.FromJson<SaveData>(save);
        return loadedData;
    }

    private SaveData CreateEmptyData()
    {
        SaveData newSaveData = new SaveData();

        newSaveData.WeaponsObtained = 0;
        newSaveData.PlayerPos = Vector3.zero;
        newSaveData.PlayerRot = Quaternion.identity;
        newSaveData.Ammo = new int[0];
        newSaveData.CheckpointTimer = 0;
        newSaveData.OnALevel = false;
        newSaveData.TimerInfo = new List<TimerData>();

        return newSaveData;
    }

    public void DeleteSave()
    {
        if (!File.Exists(_fileName))
        {
            Debug.Log("No hay save guardado");
            return;
        }

        File.Delete(_fileName);
        CreateFile();
        GameManager.GetInstance.LoadData();
    }
}
