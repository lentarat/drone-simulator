using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    void ISceneLoader.LoadScene(SceneType sceneType)
    {
        string sceneName = sceneType.ToString();
        SceneManager.LoadScene(sceneName);
    }
}
