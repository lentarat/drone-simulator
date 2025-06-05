using UnityEngine;

public abstract class DroneFlightModeMovementAdjusterStrategy
{
    protected float TiltAngleThreshold { get; private set; }

    public void SetTiltAngleThreshold(float tiltAngle)
    {
        TiltAngleThreshold = tiltAngle;
    }

    public virtual Vector2 GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        return rawInputVector;
    }
}
