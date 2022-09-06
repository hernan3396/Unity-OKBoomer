using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    #region Transitions
    [SerializeField] private int _transitionSpeed = 3;
    [SerializeField] private bool _isMenu = false;
    #endregion

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

        yield return new WaitForSeconds(0.1f); // que espere un poquito asi carga la escena antes de lanzar ese evento
        EventManager.OnGameStart();
    }

    public IEnumerator Transition(string scene)
    {
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);
        LoadLevel(scene);
    }

    public void OnNextLevel(string scene)
    {
        StartCoroutine("ChangingLevel", scene);
    }

    private IEnumerator ChangingLevel(string scene)
    {
        EventManager.OnNextLevel();
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);

        LoadLevel(scene);
    }

    public void StartTransition(string scene)
    {
        StartCoroutine("Transition", scene);
    }

    private void OnDestroy()
    {
        EventManager.Pause -= OnPause;
    }
}