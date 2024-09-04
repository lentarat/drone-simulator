using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovementInputActionsReader
{
    public InputActions InputActions { private set; get; }
    public Vector2 GetPitchAndRollInputValue => InputActions.Drone.PitchAndRoll.ReadValue<Vector2>();
    public float GetYawInputValue => InputActions.Drone.Yaw.ReadValue<float>();
    public float GetThrottleInputValue => InputActions.Drone.Throttle.ReadValue<float>();

    public DroneMovementInputActionsReader()
    {
        InputActions = new InputActions();
    }

    public void Enable()
    {
        InputActions.Drone.Enable();
    }

    public void Disable()
    {
        InputActions.Drone.Disable();
    }
}
