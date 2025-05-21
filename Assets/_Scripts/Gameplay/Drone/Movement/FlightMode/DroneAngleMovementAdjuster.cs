using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAngleMovementAdjuster : IDroneFlightModeMovementAdjuster
{
    private float _tiltAngleThreshold;
    private float _antiTiltInputValue = 0.2f;
    private float _stopStabilizingRange = 5f;

    float IDroneFlightModeMovementAdjuster.ActionAngleThreshold
    {
        set => _tiltAngleThreshold = value;
    }

    Vector2 IDroneFlightModeMovementAdjuster.GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        Vector2 adjustedInputVector = rawInputVector;
        Vector3 droneRotationEuler = droneRotation.eulerAngles;
        float zRot = droneRotationEuler.z;
        float xRot = droneRotationEuler.x;

        if (rawInputVector == Vector2.zero)
        {
            adjustedInputVector = GetStabilizationInputVector(xRot, zRot);
            return adjustedInputVector;
        }

        AdjustInput(zRot, ref adjustedInputVector.y);
        AdjustInput(xRot, ref adjustedInputVector.x);
        
        return adjustedInputVector;
    }

    private Vector2 GetStabilizationInputVector(float xRot, float zRot)
    {
        Vector2 stabilizedInputVector = Vector2.zero;
        if (zRot > 180)
        {
            float offsetFromStabilizedValue = 360 - zRot;
            float interpolatedInputValueY = -1;
            if (offsetFromStabilizedValue < _stopStabilizingRange)
            {
                interpolatedInputValueY = 0;
            }
            stabilizedInputVector.y = interpolatedInputValueY;
        }
        else if (zRot < 180)
        {
            float offsetFromStabilizedValue = zRot;
            float interpolatedInputValueY = 1;
            if(offsetFromStabilizedValue < _stopStabilizingRange)
            {
                interpolatedInputValueY = 0;
            }
            stabilizedInputVector.y = interpolatedInputValueY;
        }

        if (xRot > 180)
        {
            float offsetFromStabilizedValue = 360 - xRot;
            float interpolatedInputValueX = -1;
            if (offsetFromStabilizedValue < _stopStabilizingRange)
            {
                interpolatedInputValueX = 0;
            }
            stabilizedInputVector.x = interpolatedInputValueX;
        }
        else if (xRot < 180)
        {
            float offsetFromStabilizedValue = xRot;
            float interpolatedInputValueX = 1;
            if (offsetFromStabilizedValue < _stopStabilizingRange)
            {
                interpolatedInputValueX = 0;
            }
            stabilizedInputVector.x = interpolatedInputValueX;
        }

        return stabilizedInputVector;
    }
    
    private void AdjustInput(float rot, ref float inputValue)
    {
        if (rot > _tiltAngleThreshold && rot < 360 - _tiltAngleThreshold)
        {
            if (rot - 180 < 0)
            {
                inputValue = _antiTiltInputValue;
            }
            else
            {
                inputValue = -_antiTiltInputValue;
            }
        }
    }
}
