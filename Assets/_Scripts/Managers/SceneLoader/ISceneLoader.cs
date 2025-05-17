using System;

public interface ISceneLoader 
{
    event Action OnSceneChanged;
    void LoadScene(SceneType sceneType);
}
