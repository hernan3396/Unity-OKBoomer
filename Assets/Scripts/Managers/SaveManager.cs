using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    private Player _player;
    // juntar con saves manager luego
    private void Awake()
    {
        EventManager.GameSaved += SaveGame;
        EventManager.GameLoad += LoadGame;
    }

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();
    }

    private void SaveGame()
    {
        SaveData save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    private void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.LogWarning("No game file saved!");
            return;
        }


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        SaveData save = (SaveData)bf.Deserialize(file);
        file.Close();

        // pasar esta data al player por evento como parametro

        // si no hay data en este nivel no enviar esta info
        if (save.HasData)
            _player.SetLoadedInfo(save);

        Debug.Log(save.Ammo[1]);
        Debug.Log(save.Ammo[2]);
        Debug.Log("Game Loaded");
    }

    public void SaveAsJSON()
    {
        SaveData save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);
    }

    private SaveData CreateSaveGameObject()
    {
        SaveData save = new SaveData();

        save.SavePlayerPosition(_player.Transform.position);
        save.Ammo = _player.GetBullets;

        return save;
    }

    private void OnDestroy()
    {
        EventManager.GameSaved -= SaveGame;
        EventManager.GameLoad -= LoadGame;
    }
}
