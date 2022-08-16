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
    #endregion
}
