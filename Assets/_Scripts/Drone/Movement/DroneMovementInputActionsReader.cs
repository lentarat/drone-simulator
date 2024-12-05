using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovementInputActionsReader : IDroneMoveable
{
    public InputActions InputActions { private set; get; }
    Vector2 IDroneMoveable.GetPitchAndRollInputValue => InputActions.Drone.PitchAndRoll.ReadValue<Vector2>();
    float IDroneMoveable.GetYawInputValue => InputActions.Drone.Yaw.ReadValue<float>();
    float IDroneMoveable.GetThrottleInputValue => InputActions.Drone.Throttle.ReadValue<float>();

    public DroneMovementInputActionsReader()
    {
        InputActions = new InputActions();
        InputActions.Drone.Enable();
    }

    ~DroneMovementInputActionsReader()
    {
        InputActions.Drone.Disable();
    }
}
