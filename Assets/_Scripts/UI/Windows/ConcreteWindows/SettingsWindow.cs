using UnityEngine;

public class SettingsWindow : BackableWindow
{
    public override void Initialize()
    {
        base.Initialize();

        StayAliveInPlayMode();
    }

    private void StayAliveInPlayMode()
    {
        Transform topMostParent = transform.root; 
        DontDestroyOnLoad(topMostParent);
    }
}
