using System;
using UnityEngine.InputSystem;

public class DroneCameraSwitchInputActionsReader : IDroneCameraSwitchInvoker
{
    private InputActions _inputActions;
    private event Action _onCameraSwitched;

    public DroneCameraSwitchInputActionsReader(InputActions inputActions)
    {
        _inputActions = inputActions;
        SubscribeToCameraSwitchCalled();
    }

    private void SubscribeToCameraSwitchCalled()
    {
        _inputActions.Drone.CameraSwitch.started += InvokeCameraSwitched;
    }

    private void InvokeCameraSwitched(InputAction.CallbackContext obj)
    {
        _onCameraSwitched?.Invoke();
    }

    ~DroneCameraSwitchInputActionsReader()
    {
        UnsubscribeToCameraSwitchCalled();
    }

    private void UnsubscribeToCameraSwitchCalled()
    {
        _inputActions.Drone.CameraSwitch.started -= InvokeCameraSwitched;
    }

    event Action IDroneCameraSwitchInvoker.OnCameraSwitchCalled
    {
        add
        {
            _onCameraSwitched += value;
        }

        remove
        {
            _onCameraSwitched -= value;
        }
    }
}
