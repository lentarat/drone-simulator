using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHorizonMovementAdjuster : DroneFlightModeMovementAdjuster
{
    const float _inputVectorDeadzone = 0.01f;
    const float _flipThresholdMultiplier = 0.975f;
    const float _flipStrength = 2f;

    private float _currentRollAngle;
    private float _currentPitchAngle;
    private Vector3 _droneRight;
    private Vector3 _droneForward;
    private RollingDirection _currentRollingDirection;

    private enum RollingDirection
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

        bool isStillRolling = IsStillRolling();
        if (isStillRolling)
        {
            deltaVector = GetRollingDeltaVector();
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

            Debug.Log(_currentPitchAngle + " " +_droneForward.z);
            if (Mathf.Abs(_currentRollAngle) > TiltAngleThreshold * _flipThresholdMultiplier)
            {
                deltaRoll = Mathf.Sign(inputVector.x) * _flipStrength;
                SetRollRollingDirection(_currentRollAngle);
            }
            else
            {
                deltaRoll = Mathf.Clamp(deltaRoll / TiltAngleThreshold, -1f, 1f);
                SetPitchRollingDirection(_currentPitchAngle);
            }

            if (Mathf.Abs(_currentPitchAngle) > TiltAngleThreshold * _flipThresholdMultiplier)
            {
                deltaPitch = Mathf.Sign(inputVector.y) * _flipStrength;
            }
            else
            {
                deltaPitch = Mathf.Clamp(deltaPitch / TiltAngleThreshold, -1f, 1f);
            }
        }

        deltaVector = new Vector2(deltaRoll, deltaPitch);
        return deltaVector;
    }

    private bool IsStillRolling()
    {
        if (_currentRollingDirection == RollingDirection.Forward)
        {
            if (_currentPitchAngle > 0 && _droneForward.z < 0)
            {
                Debug.Log("Rolling over");
            }
        }
         return false;
    }

    private Vector2 GetRollingDeltaVector()
    {
        Vector2 rollingDeltaVector = _currentRollingDirection switch
        {
            RollingDirection.Forward => new Vector2(0f, 1f),
            RollingDirection.Right => new Vector2(1f, 0f),
            RollingDirection.Backward => new Vector2(0f, -1f),
            RollingDirection.Left => new Vector2(-1f, 0f),
            _ => Vector2.zero
        };

        return rollingDeltaVector;
    }

    private void SetRollRollingDirection(float currentRollAngle)
    {
        if (currentRollAngle > 0f)
        {
            _currentRollingDirection = RollingDirection.Right;
        }
        else
        {
            _currentRollingDirection = RollingDirection.Left;
        }
    }

    private void SetPitchRollingDirection(float currentPitchAngle)
    {
        if (currentPitchAngle > 0f)
        {
            _currentRollingDirection = RollingDirection.Forward;
        }
        else
        {
            _currentRollingDirection = RollingDirection.Backward;
        }
    }

    //private float _rollInputValue = 3f;
    //private RollDirectionType _rollDirectionType;

    //private enum RollDirectionType
    //{
    //    None,
    //    Forward,
    //    Backward,
    //    Right,
    //    Left
    //}

    //public override Vector2 GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    //{
    //    Vector2 adjustedInputVector = rawInputVector;

    //    Vector3 droneRotationEuler = droneRotation.eulerAngles;
    //    float zRot = droneRotationEuler.z;
    //    float xRot = droneRotationEuler.x;

    //    if (_rollDirectionType != RollDirectionType.None)
    //    {
    //        if(HasRollingEnded(xRot, zRot))
    //        { 
    //            _rollDirectionType = RollDirectionType.None;
    //        }

    //        adjustedInputVector = GetInputAccordingRollingDirection();
    //        return adjustedInputVector;
    //    }

    //    if (rawInputVector == Vector2.zero)
    //    {
    //        adjustedInputVector = GetStabilizationInputVector(xRot, zRot);
    //        return adjustedInputVector;
    //    }

    //    if (HasPassedRollThreshold(zRot, ref adjustedInputVector.y))
    //    {
    //        _rollDirectionType =  adjustedInputVector.y < 0 ? RollDirectionType.Backward : RollDirectionType.Forward;
    //    }
    //    if (HasPassedRollThreshold(xRot, ref adjustedInputVector.x))
    //    {
    //        _rollDirectionType = adjustedInputVector.x < 0 ? RollDirectionType.Left : RollDirectionType.Right;
    //    }

    //    return adjustedInputVector;
    //}

    //private bool HasRollingEnded(float xRot, float zRot)
    //{
    //    float dummy = 0;
    //    bool isRollingZRot = HasPassedRollThreshold(zRot, ref dummy);
    //    bool isRollingXRot = HasPassedRollThreshold(xRot, ref dummy);

    //    if (isRollingXRot == false && isRollingZRot == false)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Vector2 GetInputAccordingRollingDirection()
    //{
    //    return _rollDirectionType switch
    //    {
    //        RollDirectionType.Forward => new Vector2(0f, _rollInputValue),
    //        RollDirectionType.Backward => new Vector2(0f, -_rollInputValue),
    //        RollDirectionType.Left => new Vector2(-_rollInputValue, 0f),
    //        RollDirectionType.Right => new Vector2(_rollInputValue, 0f),
    //        _ => Vector2.zero
    //    };
    //}

    //private bool HasPassedRollThreshold(float rot, ref float inputValue)
    //{
    //    if (rot > TiltAngleThreshold && rot < 360 - TiltAngleThreshold)
    //    {
    //        if (_rollDirectionType != RollDirectionType.None)
    //        {
    //            return true;
    //        }

    //        if (rot - 180 < 0)
    //        {
    //            inputValue = -_rollInputValue;
    //            return true;
    //        }
    //        else
    //        {
    //            inputValue = _rollInputValue;
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
