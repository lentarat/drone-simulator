using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHorizonMovementAdjusterStrategy : DroneFlightModeMovementAdjusterStrategy
{
    const float _inputVectorDeadzone = 0.01f;
    const float _flipThresholdMultiplier = 0.95f;
    const float _flipStrength = 4f;

    private float _currentRollAngle;
    private float _currentPitchAngle;
    private Vector3 _droneRight;
    private Vector3 _droneForward;
    private Vector3 _droneUp;
    private FlippingDirection _currentFlippingDirection;

    private enum FlippingDirection
    {
        None,
        Forward,
        Right,
        Backward,
        Left
    }

    public override Vector2 GetAdjustedPitchAndRollInputVector(Vector2 inputVector, Quaternion droneRotation)
    {
        _droneRight = droneRotation * Vector3.right;
        _droneForward = droneRotation * Vector3.forward;
        _droneUp = droneRotation * Vector3.up;

        _currentRollAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(_droneForward, Vector3.up),
            _droneForward,
            -_droneRight
        );

        _currentPitchAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(_droneRight, Vector3.up),
            _droneRight,
            -_droneForward
        );

        bool isNeutral = inputVector.sqrMagnitude < _inputVectorDeadzone * _inputVectorDeadzone;

        float deltaRoll;
        float deltaPitch;
        Vector2 deltaVector;

        bool isStillFlipping = IsStillFlipping();
        if (isStillFlipping)
        {
            deltaVector = GetFlippingDeltaVector();
            return deltaVector;
        }

        if (isNeutral)
        {
            deltaRoll = Mathf.Clamp(-_currentRollAngle / TiltAngleThreshold, -1f, 1f);
            deltaPitch = Mathf.Clamp(-_currentPitchAngle / TiltAngleThreshold, -1f, 1f);
        }
        else
        {
            float desiredRollAngle = Mathf.Clamp(inputVector.x, -1f, 1f) * TiltAngleThreshold;
            float desiredPitchAngle = Mathf.Clamp(inputVector.y, -1f, 1f) * TiltAngleThreshold;

            deltaRoll = desiredRollAngle - _currentRollAngle;
            deltaPitch = desiredPitchAngle - _currentPitchAngle;

            if (Mathf.Abs(_currentRollAngle) > TiltAngleThreshold * _flipThresholdMultiplier)
            {
                SetRollFlippingDirection(_currentRollAngle);
            }
            else
            {
                deltaRoll = Mathf.Clamp(deltaRoll / TiltAngleThreshold, -1f, 1f);
            }

            if (Mathf.Abs(_currentPitchAngle) > TiltAngleThreshold * _flipThresholdMultiplier)
            {
                SetPitchFlippingDirection(_currentPitchAngle);
            }
            else
            {
                deltaPitch = Mathf.Clamp(deltaPitch / TiltAngleThreshold, -1f, 1f);
            }
        }

        deltaVector = new Vector2(deltaRoll, deltaPitch);
        return deltaVector;
    }

    private bool IsStillFlipping()
    {
        if (_currentFlippingDirection == FlippingDirection.None)
        {
            return false;
        }

        switch (_currentFlippingDirection)
        {
            case FlippingDirection.Forward:
                {
                    if (_currentPitchAngle < 0f && _currentPitchAngle > -TiltAngleThreshold && _droneUp.y > 0)
                    {
                        _currentFlippingDirection = FlippingDirection.None;
                    }
                    break;
                }
            case FlippingDirection.Left:
                {
                    if (_currentRollAngle > 0f && _currentRollAngle < TiltAngleThreshold && _droneUp.y > 0)
                    {
                        _currentFlippingDirection = FlippingDirection.None;
                    }
                    break;
                }
            case FlippingDirection.Backward:
                {
                    if (_currentPitchAngle > 5f && _currentPitchAngle < TiltAngleThreshold && _droneUp.y > 0)
                    {
                        _currentFlippingDirection = FlippingDirection.None;
                    }
                    break;
                }
            case FlippingDirection.Right:
                {
                    if (_currentRollAngle < -5f && _currentRollAngle > -TiltAngleThreshold && _droneUp.y > 0)
                    {
                        _currentFlippingDirection = FlippingDirection.None;
                    }
                    break;
                }
        }

        return true;
    }

    private Vector2 GetFlippingDeltaVector()
    {
        Vector2 flippingDeltaVector = _currentFlippingDirection switch
        {
            FlippingDirection.Forward => new Vector2(0f, _flipStrength),
            FlippingDirection.Right => new Vector2(_flipStrength, 0f),
            FlippingDirection.Backward => new Vector2(0f, -_flipStrength),
            FlippingDirection.Left => new Vector2(-_flipStrength, 0f),
            _ => Vector2.zero
        };

        return flippingDeltaVector;
    }

    private void SetRollFlippingDirection(float currentRollAngle)
    {
        if (currentRollAngle > 0f)
        {
            _currentFlippingDirection = FlippingDirection.Right;
        }
        else
        {
            _currentFlippingDirection = FlippingDirection.Left;
        }
    }

    private void SetPitchFlippingDirection(float currentPitchAngle)
    {
        if (currentPitchAngle > 0f)
        {
            _currentFlippingDirection = FlippingDirection.Forward;
        }
        else
        {
            _currentFlippingDirection = FlippingDirection.Backward;
        }
    }
}
