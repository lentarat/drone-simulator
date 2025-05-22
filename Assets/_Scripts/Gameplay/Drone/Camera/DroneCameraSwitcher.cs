using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DroneCameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera _frontCamera;
    [SerializeField] private Camera _bottomCamera;

    private IDroneCameraSwitchInvoker _droneCameraSwitchInvoker;

    [Inject]
    private void Construct(IDroneCameraSwitchInvoker droneCameraSwitchInvoker)
    {
        _droneCameraSwitchInvoker = droneCameraSwitchInvoker;
        SubscribeToCameraSwitchCalled();
    }

    private void SubscribeToCameraSwitchCalled()
    {
        _droneCameraSwitchInvoker.OnCameraSwitchCalled += SwitchCamera;
    }

    private void SwitchCamera()
    {
        bool isFrontCameraActive = _frontCamera.gameObject.activeInHierarchy;
        _frontCamera.gameObject.SetActive(!isFrontCameraActive);
        bool isBottonCameraActive = _bottomCamera.gameObject.activeInHierarchy;
        _bottomCamera.gameObject.SetActive(!isBottonCameraActive);
    }

    private void OnDestroy()
    {
        UnsubscribeToCameraSwitchCalled();
    }

    private void UnsubscribeToCameraSwitchCalled()
    {
        _droneCameraSwitchInvoker.OnCameraSwitchCalled -= SwitchCamera;
    }
}
