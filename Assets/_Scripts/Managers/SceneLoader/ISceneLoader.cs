using System;

public interface ISceneLoader 
{
    event Action<SceneType> OnSceneChanged;
    void ReloadCurrentScene();
    void LoadScene(SceneType sceneType);
}
