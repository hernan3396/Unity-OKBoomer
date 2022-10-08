using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    #region Transitions
    [SerializeField] private int _transitionSpeed = 3;
    [SerializeField] private bool _isMenu = false;
    private string _currentLevel;
    #endregion

    private void Awake()
    {
        EventManager.ChangeLevel += OnNextLevelNoSave;
        EventManager.GameLoaded += SetContinue;
    }

    private void Start()
    {
        if (!_isMenu)
        {
            StartCoroutine("StartLevel");
            EventManager.Pause += OnPause;
        }
    }

    private void LoadLevel(string scene, bool async = false)
    {
        if (async)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
                return;
            }

            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            return;
        }

        DOTween.KillAll();
        SceneManager.LoadScene(scene);
    }

    private void OnPause(bool value)
    {
        LoadLevel("PauseMenu", true);
    }

    private IEnumerator StartLevel()
    {
        EventManager.OnStartTransitionOut(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed * 0.5f);

        LoadLevel("UI", true);
    }

    public IEnumerator Transition(string scene)
    {
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);
        LoadLevel(scene);
    }

    public void OnNextLevel(string scene)
    {
        GameManager.GetInstance.OnExit();
        StartCoroutine("ChangingLevel", scene);
    }

    private IEnumerator ChangingLevel(string scene)
    {
        EventManager.OnNextLevel();
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);

        LoadLevel(scene);
    }

    public void OnNextLevelNoSave(string scene)
    {
        StartCoroutine("ChangingLevelNoSave", scene);
    }

    private IEnumerator ChangingLevelNoSave(string scene)
    {
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);

        LoadLevel(scene);
    }

    private void SetContinue(SaveData save)
    {
        _currentLevel = save.CurrentLevel;
    }

    public void Continue()
    {
        StartCoroutine("ChangingLevelNoSave", _currentLevel);
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
        EventManager.ChangeLevel -= OnNextLevelNoSave;
        EventManager.GameLoaded -= SetContinue;
    }
}