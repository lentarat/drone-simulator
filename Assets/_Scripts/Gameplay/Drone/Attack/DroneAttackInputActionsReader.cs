using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneAttackInputActionsReader : IPayloadReleaseInvoker
{
    private InputActions _inputActions;
    private event Action _onPayloadReleaseButtonPressed;

    public DroneAttackInputActionsReader(InputActions inputActions)
    { 
        _inputActions = inputActions;
        SubscribeToPayloadReleaseCalled();
    }

    private void SubscribeToPayloadReleaseCalled()
    {
        _inputActions.Drone.PayloadRelease.canceled += HandlePayloadReleased;
    }

    private void HandlePayloadReleased(InputAction.CallbackContext obj)
    {
        _onPayloadReleaseButtonPressed?.Invoke();
    }

    ~DroneAttackInputActionsReader()
    {
        UnsubscribeToPayloadReleaseCalled();
    }

    private void UnsubscribeToPayloadReleaseCalled()
    {
        _inputActions.Drone.PayloadRelease.canceled -= HandlePayloadReleased;
    }

    event Action IPayloadReleaseInvoker.OnReleaseCalled
    {
        add 
        {
            _onPayloadReleaseButtonPressed += value;
        }
        remove
        {
            _onPayloadReleaseButtonPressed -= value;
        }
    }
}
