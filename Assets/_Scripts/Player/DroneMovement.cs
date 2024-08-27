using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    
    private InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Drone.PitchAndRoll.performed += HandlePitchAndRollChanged;
        _inputActions.Drone.Yaw.performed += HandleYawChanged;
        _inputActions.Drone.Throttle.performed += HandleThrottleChanged;
        _inputActions.Drone.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Drone.PitchAndRoll.performed -= HandlePitchAndRollChanged;
        _inputActions.Drone.Yaw.performed -= HandleYawChanged;
        _inputActions.Drone.Throttle.performed -= HandleThrottleChanged;
       _inputActions.Drone.Disable();
    }

    private void Update()
    {
        //Debug.Log("\t" + _inputActions.Drone.PitchAndRoll.ReadValue<Vector2>());
    }

    private void HandlePitchAndRollChanged(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }

    private void HandleYawChanged(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
    }

    private void HandleThrottleChanged(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
    }
}
