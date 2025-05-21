using System;

public interface ISceneLoader 
{
    event Action OnSceneChanged;
    void ReloadCurrentScene();
    void LoadScene(SceneType sceneType);
}
