using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    #region Transitions
    [SerializeField] private int _transitionSpeed;
    #endregion

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

    public IEnumerator Transition(string scene)
    {
        EventManager.OnStartTransition(_transitionSpeed);
        yield return new WaitForSeconds(_transitionSpeed);
        LoadLevel(scene);
    }

    public void StartTransition(string scene)
    {
        StartCoroutine("Transition", scene);
    }
}