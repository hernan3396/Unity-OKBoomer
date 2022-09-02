using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [Header("Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _mainCam;
    #endregion

    #region Pools
    [Header("Pools")]
    [SerializeField] private PoolManager[] _pools;
    [SerializeField] private PoolManager[] _enemyPools;
    [SerializeField] private PoolManager[] _utilsPools;
    [SerializeField] private PoolManager[] _pickablesPools;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }

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
    #endregion
}
