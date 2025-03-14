using UnityEngine;

public class WindowsInitializator : MonoBehaviour
{
    [SerializeField] private BaseWindow[] _windows;

    private void Awake()
    {
        foreach (BaseWindow window in _windows)
        {
            window.Initialize();
        }
    }
}
