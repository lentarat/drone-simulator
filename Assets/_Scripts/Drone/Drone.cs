using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Drone : MonoBehaviour
{
    private InputActions _inputActions;

    [Inject]
    private void Construct(InputActions inputActions)
    { 
        _inputActions = inputActions;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
