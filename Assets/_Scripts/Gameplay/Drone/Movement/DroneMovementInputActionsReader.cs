using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovementInputActionsReader : IDroneMoveable
{
    private InputActions _inputActions;
    Vector2 IDroneMoveable.GetPitchAndRollInputValue => _inputActions.Drone.PitchAndRoll.ReadValue<Vector2>();
    float IDroneMoveable.GetYawInputValue => _inputActions.Drone.Yaw.ReadValue<float>();
    float IDroneMoveable.GetThrottleInputValue => _inputActions.Drone.Throttle.ReadValue<float>();

    public DroneMovementInputActionsReader(InputActions inputActions)
    {
        _inputActions = inputActions;
    }
}
