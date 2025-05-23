using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByIndex (int sceneIndex)
    {
        Scene currentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);

        if (currentScene != null)
            SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
