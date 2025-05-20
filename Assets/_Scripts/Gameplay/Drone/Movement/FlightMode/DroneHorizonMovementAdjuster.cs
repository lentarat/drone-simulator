using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHorizonMovementAdjuster : IDroneFlightModeMovementAdjuster
{
    private float _rollAngleThreshold;
    private RollDirectionType _rollDirectionType;

    private enum RollDirectionType
    {
        None,
        Forward,
        Backward,
        Right,
        Left
    }

    float IDroneFlightModeMovementAdjuster.ActionAngleThreshold
    {
        set => _rollAngleThreshold = value;
    }

    Vector2 IDroneFlightModeMovementAdjuster.GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        Vector2 adjustedInputVector = rawInputVector;

        if (_rollDirectionType != RollDirectionType.None)
        {
            adjustedInputVector = GetInputAccordingRollingDirection();
            return adjustedInputVector;
        }

        Vector3 droneRotationEuler = droneRotation.eulerAngles;
        float zRot = droneRotationEuler.z;
        float xRot = droneRotationEuler.x;

        if (rawInputVector == Vector2.zero)
        {
            adjustedInputVector = GetDefaultInputVector(xRot, zRot);
            return adjustedInputVector;
        }

        if (AdjustInput(zRot, ref adjustedInputVector.y))
        {
            _rollDirectionType =  adjustedInputVector.y < 0 ? RollDirectionType.Backward : RollDirectionType.Forward;
        }
        if (AdjustInput(xRot, ref adjustedInputVector.x))
        {
            _rollDirectionType = adjustedInputVector.x < 0 ? RollDirectionType.Left : RollDirectionType.Right;
        }

        return adjustedInputVector;
    }

    private Vector2 GetDefaultInputVector(float xRot, float zRot)
    {
        if (zRot != 0f)
        {
            int a = 5;
        }
        xRot = Mathf.Lerp(xRot, 0f, 0.01f * Time.deltaTime);
        xRot = Mathf.CeilToInt(xRot);
        zRot = Mathf.Lerp(zRot, 0f, 0.01f * Time.deltaTime);
        zRot = Mathf.CeilToInt(zRot);
        return new Vector2(xRot, zRot);
    }

    private Vector2 GetInputAccordingRollingDirection()
    {
        return _rollDirectionType switch
        {
            RollDirectionType.Forward => new Vector2(0f, 1f),
            RollDirectionType.Backward => new Vector2(0f, -1f),
            RollDirectionType.Left => new Vector2(-1f, 0f),
            RollDirectionType.Right => new Vector2(1f, 0f),
            _ => Vector2.zero
        };
    }

    private bool AdjustInput(float rot, ref float inputValue)
    {
        if (rot > _rollAngleThreshold && rot < 360 - _rollAngleThreshold)
        {
            if (rot - 180 < 0)
            {
                inputValue = -1;
                return true;
            }
            else
            {
                inputValue = 1;
                return true;
            }
        }

        return false;
    }
}
