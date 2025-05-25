using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private Dictionary<string, SceneType> _stringToSceneType = new Dictionary<string, SceneType>()
    {
        { "MainMenu", SceneType.MainMenu },
        { "Map1", SceneType.Map1 }
    };
    private event Action<SceneType> _onSceneChanged;

    event Action<SceneType> ISceneLoader.OnSceneChanged
    {
        add =>_onSceneChanged += value;
        remove => _onSceneChanged -= value;
    }

    void ISceneLoader.ReloadCurrentScene()
    {
        ReloadCurrentSceneAsync().Forget();
    }

    private async UniTask ReloadCurrentSceneAsync()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        await SceneManager.LoadSceneAsync(currentSceneIndex);

        Scene currentScene = SceneManager.GetActiveScene();
        InformSceneChanged(currentScene, LoadSceneMode.Single);
    }

    void ISceneLoader.LoadScene(SceneType sceneType)
    {
        string sceneName = sceneType.ToString();
        SceneManager.sceneLoaded += InformSceneChanged;
        SceneManager.LoadScene(sceneName);
    }

    private void InformSceneChanged(Scene loadedScene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= InformSceneChanged;
        SceneType loadedSceneType = _stringToSceneType[loadedScene.name];
        _onSceneChanged?.Invoke(loadedSceneType);
    }

    SceneType ISceneLoader.GetCurrentSceneType()
    {
        string currentSceneString = SceneManager.GetActiveScene().name;
        SceneType currentScene = _stringToSceneType[currentSceneString];
        return currentScene;
    }
}
