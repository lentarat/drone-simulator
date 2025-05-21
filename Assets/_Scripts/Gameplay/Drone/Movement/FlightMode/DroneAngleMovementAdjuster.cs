using UnityEngine;

public class DroneAngleMovementAdjuster : DroneFlightModeMovementAdjuster
{
    private float _tiltAngleThreshold;
    private float _antiTiltInputValue = 0.2f;

    public override void SetActionAngleThreshold(float actionAngle)
    {
        _tiltAngleThreshold = actionAngle;
    }

    public override Vector2 GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
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
