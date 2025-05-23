using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadSceneByIndex (int sceneIndex)
    {
        Scene currentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
