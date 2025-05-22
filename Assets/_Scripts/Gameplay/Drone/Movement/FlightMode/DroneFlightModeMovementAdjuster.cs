using UnityEngine;

public abstract class DroneFlightModeMovementAdjuster
{
    private float _stopStabilizingRange = 5f;

    protected float TiltAngleThreshold { get; private set; }

    public void SetTiltAngleThreshold(float tiltAngle)
    {
        TiltAngleThreshold = tiltAngle;
    }

    public virtual Vector2 GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        return rawInputVector;
    }

    protected Vector2 GetStabilizationInputVector(float xRot, float zRot)
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
}
