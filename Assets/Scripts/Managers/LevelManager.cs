using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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

        SceneManager.LoadScene(scene);
    }
}
