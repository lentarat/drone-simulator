using System;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private event Action _onSceneChanged;

    event Action ISceneLoader.OnSceneChanged
    {
        add =>_onSceneChanged += value;
        remove => _onSceneChanged -= value;
    }

    void ISceneLoader.ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void ISceneLoader.LoadScene(SceneType sceneType)
    {
        string sceneName = sceneType.ToString();
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += InformSceneChanged;
        _onSceneChanged?.Invoke();
    }

    private void InformSceneChanged(Scene loadedScene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= InformSceneChanged;
        _onSceneChanged?.Invoke();
    }
}
