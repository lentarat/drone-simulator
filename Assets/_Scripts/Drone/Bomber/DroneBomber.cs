using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class DroneBomber : MonoBehaviour
{
    private IDroneBomber _droneBomber;

    [Inject]
    private void Construct(IDroneBomber droneBomber)
    {
        _droneBomber = droneBomber;
    }

    private void Awake()
    {
        _droneBomber.OnBombReleaseCalled += HandleBombReleaseCall;
    }

    private void HandleBombReleaseCall()
    {
        DropBomb();
    }

    private void DropBomb()
    {

    }

    private void OnDisable()
    {
        _droneBomber.OnBombReleaseCalled -= HandleBombReleaseCall;
    }
}
