using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAngleMovementAdjuster : IDroneFlightModeMovementAdjuster
{
    private float _leapAngleThreshold;
    float IDroneFlightModeMovementAdjuster.ActionAngleThreshold
    {
        set => _leapAngleThreshold = value;
    }

    Vector2 IDroneFlightModeMovementAdjuster.GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        Vector3 droneRotationEuler = droneRotation.eulerAngles;
        float zRot = droneRotationEuler.z;
        float xRot = droneRotationEuler.x;
        Vector2 adjustedInputVector = rawInputVector;
        AdjustInput(zRot, ref adjustedInputVector.y);
        AdjustInput(xRot, ref adjustedInputVector.x);
        return adjustedInputVector;
    }

    private void AdjustInput(float rot, ref float inputValue)
    {
        if (rot > _leapAngleThreshold && rot < 360 - _leapAngleThreshold)
        {
            if (rot - 180 < 0)
            {
                inputValue = 1;
            }
            else
            {
                inputValue = - 1;
            }
        }
    }
}
