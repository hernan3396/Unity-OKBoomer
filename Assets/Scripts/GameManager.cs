using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [Header("Components")]
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _mainCam;
    private Player _playerScript;
    #endregion

    #region Pools
    [Header("Pools")]
    [SerializeField] private PoolManager[] _pools;
    [SerializeField] private PoolManager[] _enemyPools;
    [SerializeField] private PoolManager[] _utilsPools;
    [SerializeField] private PoolManager[] _pickablesPools;
    #endregion

    private SaveData _saveData;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        if (_player != null)
            _playerScript = _player.GetComponent<Player>();

        _saveData = new SaveData();
    }

    private void Start()
    {
        LoadData();
    }

    private void OnDestroy()
    {
        if (_instance != null)
            _instance = null;
    }

    #region Saves
    public void OnCheckpoint()
    {
        _saveData.WeaponsObtained = _playerScript.MaxWeapons;
        _saveData.PlayerPos = _player.transform.position;
        _saveData.PlayerRot = _player.transform.rotation;
        _saveData.PlayerHealth = _playerScript.CurrentHP;
        _saveData.Ammo = _playerScript.GetBullets;

        _playerScript.Checkpoint = _player.transform.position; // un poco raro que esten aca pero whatever
        _playerScript.HealthCheckpoint = _playerScript.CurrentHP; // un poco raro que esten aca pero whatever
        _playerScript.AmmoCheckpoint = _playerScript.GetBullets; // un poco raro que esten aca pero whatever
        _playerScript.CurrentRot = _player.transform.rotation;

        _saveData.OnALevel = true;
    }

    public void SaveCheckpointTime(float time, string levelName)
    {
        _saveData.CheckpointTimer = time;
        _saveData.CurrentLevel = levelName;
    }

    public void OnLevelFinished(TimerData timerData)
    {
        bool addLevel = true;

        foreach (TimerData item in _saveData.TimerInfo)
        {
            if (item.Name == timerData.Name)
            {
                addLevel = false;

                // guarda el tiempo si es mas corto
                if (item.LevelTime > timerData.LevelTime)
                    item.LevelTime = timerData.LevelTime;

                break;
            }
        }

        if (addLevel)
            _saveData.TimerInfo.Add(timerData);

        _saveData.CheckpointTimer = 0;
        _saveData.OnALevel = false;
        OnExit();
    }

    public void LoadData()
    {
        _saveData = _saveManager.GameLoad();
        EventManager.OnGameLoaded(_saveData);
    }

    public void OnExit()
    {
        _saveManager.GameSave(_saveData);
    }
    #endregion

    public static GameManager GetInstance
    {
        get { return _instance; }
    }

    #region Setter/Getter
    public Camera MainCam
    {
        get { return _mainCam; }
    }

    public GameObject Player
    {
        get { return _player; }
    }

    public PoolManager[] GetPools
    {
        get { return _pools; }
    }

    public PoolManager[] GetEnemyPools
    {
        get { return _enemyPools; }
    }

    public PoolManager GetUtilsPool(int index)
    {
        return _utilsPools[index];
    }

    public PoolManager[] GetPickablesPools
    {
        get { return _pickablesPools; }
    }

    public SaveData GetSaveData
    {
        get { return _saveData; }
    }
    #endregion
}
