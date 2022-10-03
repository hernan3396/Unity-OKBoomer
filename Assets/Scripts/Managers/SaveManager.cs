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
    private Player _player;
    // juntar con saves manager luego
    private void Awake()
    {
        EventManager.GameSaved += SaveGame;
        EventManager.GameLoad += LoadGame;
        EventManager.SaveTime += NextLevel;
    }

    private void Start()
    {
        if (GameManager.GetInstance.Player != null)
            _player = GameManager.GetInstance.Player.GetComponent<Player>();
        else
            LoadMenuData();
        // esto funciona, pero sino cambiarlo luego por algo mas normal
        // el unico lugar donde no esta el player pero si este script es en el menu
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
            _player.NoSaveInfo();
            Debug.LogWarning("No game file saved!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        SaveData save = (SaveData)bf.Deserialize(file);
        file.Close();

        // si no hay data en este nivel no enviar esta info
        if (!save.HasData) return;

        _player.SetLoadedInfo(save);

        Debug.Log("Game Loaded");
    }

    public void NewLevel(string level)
    {
        CleanSave();
        EventManager.OnChangeLevel(level);
    }

    public void Continue()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            _player.NoSaveInfo();
            Debug.LogWarning("No game file saved!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        SaveData save = (SaveData)bf.Deserialize(file);
        file.Close();

        EventManager.OnChangeLevel(save.CurrentLevelName);
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
        save.CurrentLevelName = SceneManager.GetActiveScene().name;
        save.HasData = true;

        return save;
    }

    /// <Summary>
    /// FULL DELETE
    /// </Summary>
    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            File.Delete(Application.persistentDataPath + "/gamesave.save");

        if (Directory.Exists(Application.persistentDataPath + "/levels"))
        {
            string[] timersFileName = Directory.GetFiles(Application.persistentDataPath + "/levels", "*.save");

            foreach (string fileName in timersFileName)
                File.Delete(fileName);
        }
    }

    /// <Summary>
    /// Borra la data cuando cambias entre niveles, nada muy grave
    /// </Summary>
    private void CleanSave()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.LogWarning("No game file saved!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        SaveData save = (SaveData)bf.Deserialize(file);
        file.Close();

        save.HasData = false;

        file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }

    private void NextLevel(string levelName, float levelTime)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        // preguntar si el archivo existe
        if (File.Exists(Application.persistentDataPath + "/levels/" + levelName + ".save"))
        {
            // lo carga
            file = File.Open(Application.persistentDataPath + "/levels/" + levelName + ".save", FileMode.Open);
            TimerData timerData = (TimerData)bf.Deserialize(file);
            file.Close();

            // compara el tiempo
            if (levelTime < timerData.LevelTime)
                timerData.LevelTime = levelTime;

            file = File.Create(Application.persistentDataPath + "/levels/" + levelName + ".save");
            bf.Serialize(file, timerData); // lo guarda
        }
        else
        {
            // crea la carpeta si no existe
            if (!Directory.Exists(Application.persistentDataPath + "/levels/"))
                Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

            // crea el archivo
            file = File.Create(Application.persistentDataPath + "/levels/" + levelName + ".save");

            // guarda la data
            TimerData newTimer = new TimerData();
            newTimer.Name = levelName;
            newTimer.LevelTime = levelTime;
            bf.Serialize(file, newTimer);
        }

        // cerrar el archivo
        file.Close();

        CleanSave();
    }

    private void LoadMenuData()
    {
        // level times data
        if (!Directory.Exists(Application.persistentDataPath + "/levels"))
        {
            Debug.LogWarning("No timers!");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string[] timersFileName = Directory.GetFiles(Application.persistentDataPath + "/levels", "*.save");

        float[] timers = new float[timersFileName.Length];
        int i = 0;

        foreach (string fileName in timersFileName)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            TimerData timerData = (TimerData)bf.Deserialize(file);
            timers[i] = timerData.LevelTime;
            file.Close();
            i++;
        }

        EventManager.OnLoadTimer(timers);

        // player has data?
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.LogWarning("No game file saved!");
            return;
        }

        FileStream playerData = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        SaveData save = (SaveData)bf.Deserialize(playerData);
        playerData.Close();

        if (save.HasData) EventManager.OnActivateContinueBtn();
    }

    private void OnDestroy()
    {
        EventManager.GameSaved -= SaveGame;
        EventManager.GameLoad -= LoadGame;
        EventManager.SaveTime -= NextLevel;
    }
}
