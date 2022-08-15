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

    private void Awake()
    {
        StartLevel();
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

    private void StartLevel()
    {
        if (_isMenu) return;
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
        // StartCoroutine("ChangingLevel", scene);
        EventManager.OnNextLevel();
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
}