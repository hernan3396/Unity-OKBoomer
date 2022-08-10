using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [Header("Components")]
    [SerializeField] private Camera _mainCam;
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
    #endregion
}
