using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    // hay muchas cosas repetidas en este script, pero queria
    // familiarizarme con la estructura de guardar/cargar data
    // asi que lo repeti varias veces. Si bien no creo que pase,
    // habria que ordenarlo un poco ya que hay cosas repetidas que 
    // se podrian pasar a un metodo para no tener que repetir codigo

    // en vez de sobreescribir todo el texto, sobreescribir los valores que queres cambiar
    private Player _player;
    private FileStream _fileStream;
    private string _fileName = "data";

    private void Start()
    {
        EventManager.GameSaved += GameSave;
        EventManager.GameLoad += GameLoad;
        // EventManager.SaveTime += NextLevel;

        // if (GameManager.GetInstance.Player != null)
        //     _player = GameManager.GetInstance.Player.GetComponent<Player>();
        // else
        //     LoadMenuData();
        // // esto funciona, pero sino cambiarlo luego por algo mas normal
        // // el unico lugar donde no esta el player pero si este script es en el menu
        _fileName = Application.persistentDataPath + "/" + _fileName + ".save";

        CreateFile();
    }

    // private void SaveGame()
    // {
    //     SaveData save = CreateSaveGameObject();

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
    //     bf.Serialize(file, save);
    //     file.Close();

    //     Debug.Log("Game Saved");
    // }

    // private void LoadGame()
    // {
    //     if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //     {
    //         _player.NoSaveInfo();
    //         Debug.LogWarning("No game file saved!");
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //     SaveData save = (SaveData)bf.Deserialize(file);
    //     file.Close();

    //     // si no hay data en este nivel no enviar esta info
    //     if (!save.HasData) return;

    //     _player.SetLoadedInfo(save);

    //     Debug.Log("Game Loaded");
    // }

    // public void NewLevel(string level)
    // {
    //     CleanSave(false);
    //     EventManager.OnChangeLevel(level);
    // }

    // public void Continue()
    // {
    //     if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //     {
    //         _player.NoSaveInfo();
    //         Debug.LogWarning("No game file saved!");
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //     SaveData save = (SaveData)bf.Deserialize(file);
    //     file.Close();

    //     EventManager.OnChangeLevel(save.CurrentLevelName);
    // }

    // public void SaveAsJSON()
    // {
    //     SaveData save = CreateSaveGameObject();
    //     string json = JsonUtility.ToJson(save);

    //     BinaryFormatter bf = new BinaryFormatter();
    //     if (!File.Exists(Application.persistentDataPath + "/gamesaveJson.save"))
    //     {
    //         FileStream newFile = File.Create(Application.persistentDataPath + "/gamesaveJson.save");
    //         newFile.Close();
    //     }

    //     FileStream file = File.Open(Application.persistentDataPath + "/gamesaveJson.save", FileMode.Open);
    //     bf.Serialize(file, json);
    //     file.Close();
    // }

    // public void LoadJson()
    // {
    //     if (!File.Exists(Application.persistentDataPath + "/gamesaveJson.save"))
    //     {
    //         Debug.LogWarning("No hay archivo");
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Open(Application.persistentDataPath + "/gamesaveJson.save", FileMode.Open);
    //     string save = (string)bf.Deserialize(file);
    //     file.Close();

    //     SaveData saveData = JsonUtility.FromJson<SaveData>(save);

    //     Debug.Log(saveData.PlayerPos);
    // }

    // private SaveData CreateSaveGameObject()
    // {
    //     // sin esta parte lo vuelve a poner en 0 al current max level
    //     int currentMaxLevel = 0;

    //     // if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //     // {
    //     //     BinaryFormatter bf = new BinaryFormatter();
    //     //     FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //     //     SaveData oldSave = (SaveData)bf.Deserialize(file);
    //     //     currentMaxLevel = oldSave.MaxLevelUnlocked;
    //     //     file.Close();
    //     // }
    //     // else
    //     // {
    //     //     currentMaxLevel = 0;
    //     // }

    //     SaveData save = new SaveData();

    //     save.SavePlayerPosition(_player.Transform.position);
    //     save.Ammo = _player.GetBullets;
    //     save.CurrentLevelName = SceneManager.GetActiveScene().name;
    //     save.HasData = true;
    //     save.MaxLevelUnlocked = currentMaxLevel;

    //     return save;
    // }

    // /// <Summary>
    // /// FULL DELETE
    // /// </Summary>
    // public void DeleteSave()
    // {
    //     if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //         File.Delete(Application.persistentDataPath + "/gamesave.save");

    //     if (Directory.Exists(Application.persistentDataPath + "/levels/"))
    //     {
    //         string[] timersFileName = Directory.GetFiles(Application.persistentDataPath + "/levels/", "*.save");

    //         foreach (string fileName in timersFileName)
    //             File.Delete(fileName);
    //     }
    // }

    // /// <Summary>
    // /// cambia HasData = false cuando cambias entre niveles, nada muy grave
    // /// </Summary>
    // private void CleanSave(bool updateMaxLevel)
    // {
    //     if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //     {
    //         Debug.LogWarning("No game file saved!");
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file;

    //     file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //     SaveData save = (SaveData)bf.Deserialize(file);
    //     file.Close();

    //     save.HasData = false;
    //     if (updateMaxLevel) save.MaxLevelUnlocked += 1;

    //     file = File.Create(Application.persistentDataPath + "/gamesave.save");
    //     bf.Serialize(file, save);
    //     file.Close();
    // }

    // private void NextLevel(string levelName, float levelTime)
    // {
    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file;
    //     bool _addMaxLevel = true;

    //     // preguntar si el archivo existe
    //     if (File.Exists(Application.persistentDataPath + "/levels/" + levelName + ".save"))
    //     {
    //         // lo carga
    //         file = File.Open(Application.persistentDataPath + "/levels/" + levelName + ".save", FileMode.Open);
    //         TimerData timerData = (TimerData)bf.Deserialize(file);
    //         file.Close();

    //         // compara el tiempo
    //         if (levelTime < timerData.LevelTime)
    //             timerData.LevelTime = levelTime;

    //         file = File.Create(Application.persistentDataPath + "/levels/" + levelName + ".save");
    //         bf.Serialize(file, timerData); // lo guarda

    //         _addMaxLevel = false;
    //     }
    //     else
    //     {
    //         // crea la carpeta si no existe
    //         if (!Directory.Exists(Application.persistentDataPath + "/levels/"))
    //             Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

    //         // crea el archivo
    //         file = File.Create(Application.persistentDataPath + "/levels/" + levelName + ".save");

    //         // guarda la data
    //         TimerData newTimer = new TimerData(levelName, levelTime);
    //         bf.Serialize(file, newTimer);
    //     }

    //     // cerrar el archivo
    //     file.Close();

    //     CleanSave(_addMaxLevel);
    // }

    // private void LoadMenuData()
    // {
    //     // level times data
    //     if (!Directory.Exists(Application.persistentDataPath + "/levels"))
    //     {
    //         Debug.LogWarning("No timers!");
    //         EventManager.OnActivateLevels(0);
    //         return;
    //     }

    //     BinaryFormatter bf = new BinaryFormatter();
    //     string[] timersFileName = Directory.GetFiles(Application.persistentDataPath + "/levels", "*.save");

    //     float[] timers = new float[timersFileName.Length];
    //     int i = 0;

    //     foreach (string fileName in timersFileName)
    //     {
    //         FileStream file = File.Open(fileName, FileMode.Open);
    //         TimerData timerData = (TimerData)bf.Deserialize(file);
    //         timers[i] = timerData.LevelTime;
    //         file.Close();
    //         i++;
    //     }

    //     EventManager.OnLoadTimer(timers);

    //     // player has data?
    //     if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //     {
    //         Debug.LogWarning("No game file saved!");
    //         return;
    //     }

    //     FileStream playerData = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //     SaveData save = (SaveData)bf.Deserialize(playerData);
    //     playerData.Close();

    //     if (save.HasData) EventManager.OnActivateContinueBtn();
    //     EventManager.OnActivateLevels(save.MaxLevelUnlocked);
    // }

    private void CreateFile()
    {
        if (!File.Exists(_fileName))
        {
            _fileStream = File.Create(_fileName);
            _fileStream.Close();
            return;
        }
    }

    private void GameSave(SaveData save)
    {
        string json = JsonUtility.ToJson(save);

        BinaryFormatter bf = new BinaryFormatter();
        _fileStream = File.Open(_fileName, FileMode.Open);
        bf.Serialize(_fileStream, json);
        _fileStream.Close();
    }

    private void GameLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();

        _fileStream = File.Open(_fileName, FileMode.Open);
        string save = (string)bf.Deserialize(_fileStream);
        _fileStream.Close();

        SaveData loadedData = JsonUtility.FromJson<SaveData>(save);
        EventManager.OnGameLoaded(loadedData);
    }

    private void OnDestroy()
    {
        EventManager.GameSaved -= GameSave;
        EventManager.GameLoad -= GameLoad;
        // EventManager.SaveTime -= NextLevel;
    }
}
