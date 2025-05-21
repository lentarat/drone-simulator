using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHorizonMovementAdjuster : IDroneFlightModeMovementAdjuster
{
    private float _rollAngleThreshold;
    private float _rollInputValue = 3f;
    private float _stopStabilizingRange = 5f;
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

        Vector3 droneRotationEuler = droneRotation.eulerAngles;
        float zRot = droneRotationEuler.z;
        float xRot = droneRotationEuler.x;

        if (_rollDirectionType != RollDirectionType.None)
        {
            if(HasRollingEnded(xRot, zRot))
            { 
                _rollDirectionType = RollDirectionType.None;
            }

            adjustedInputVector = GetInputAccordingRollingDirection();
            return adjustedInputVector;
        }

        if (rawInputVector == Vector2.zero)
        {
            adjustedInputVector = GetStabilizationInputVector(xRot, zRot);
            return adjustedInputVector;
        }

        if (HasPassedRollThreshold(zRot, ref adjustedInputVector.y))
        {
            _rollDirectionType =  adjustedInputVector.y < 0 ? RollDirectionType.Backward : RollDirectionType.Forward;
        }
        if (HasPassedRollThreshold(xRot, ref adjustedInputVector.x))
        {
            _rollDirectionType = adjustedInputVector.x < 0 ? RollDirectionType.Left : RollDirectionType.Right;
        }

        return adjustedInputVector;
    }

    private bool HasRollingEnded(float xRot, float zRot)
    {
        float dummy = 0;
        bool isRollingZRot = HasPassedRollThreshold(zRot, ref dummy);
        bool isRollingXRot = HasPassedRollThreshold(xRot, ref dummy);

        if (isRollingXRot == false && isRollingZRot == false)
        {
            return true;
        }

        return false;
    }

    private Vector2 GetInputAccordingRollingDirection()
    {
        return _rollDirectionType switch
        {
            RollDirectionType.Forward => new Vector2(0f, _rollInputValue),
            RollDirectionType.Backward => new Vector2(0f, -_rollInputValue),
            RollDirectionType.Left => new Vector2(-_rollInputValue, 0f),
            RollDirectionType.Right => new Vector2(_rollInputValue, 0f),
            _ => Vector2.zero
        };
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
            if (offsetFromStabilizedValue < _stopStabilizingRange)
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

    private bool HasPassedRollThreshold(float rot, ref float inputValue)
    {
        if (rot > _rollAngleThreshold && rot < 360 - _rollAngleThreshold)
        {
            if (_rollDirectionType != RollDirectionType.None)
            {
                return true;
            }

            if (rot - 180 < 0)
            {
                inputValue = -_rollInputValue;
                return true;
            }
            else
            {
                inputValue = _rollInputValue;
                return true;
            }
        }

        return false;
    }
}
