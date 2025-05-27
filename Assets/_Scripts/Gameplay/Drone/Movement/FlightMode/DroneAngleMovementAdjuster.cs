using UnityEngine;

public class DroneAngleMovementAdjuster : DroneFlightModeMovementAdjuster
{
    const float _inputVectorDeadzone = 0.01f;

    public override Vector2 GetAdjustedPitchAndRollInputVector(Vector2 inputVector, Quaternion droneRotation)
    {
        Vector3 droneRight = droneRotation * Vector3.right;   
        Vector3 droneForward = droneRotation * Vector3.forward; 

        float currentPitchAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(droneForward, Vector3.up),
            droneForward,
            -droneRight
        );

        float currentRollAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(droneRight, Vector3.up),
            droneRight,
            -droneForward
        );

        bool isNeutral = inputVector.sqrMagnitude < _inputVectorDeadzone * _inputVectorDeadzone;

        float deltaPitch;
        float deltaRoll;

        if (isNeutral)
        {
            deltaPitch = Mathf.Clamp(-currentPitchAngle / TiltAngleThreshold, -1f, 1f);
            deltaRoll = Mathf.Clamp(-currentRollAngle / TiltAngleThreshold, -1f, 1f);
        }
        else
        {
            float desiredPitchAngle = Mathf.Clamp(inputVector.x, -1f, 1f) * TiltAngleThreshold;
            float desiredRollAngle = Mathf.Clamp(inputVector.y, -1f, 1f) * TiltAngleThreshold;

            deltaPitch = Mathf.Clamp(
                (desiredPitchAngle - currentPitchAngle) / TiltAngleThreshold,
                -1f, 1f
            );

            deltaRoll = Mathf.Clamp(
                (desiredRollAngle - currentRollAngle) / TiltAngleThreshold,
                -1f, 1f
            );
        }

        return new Vector2(deltaPitch, deltaRoll);
    }
}