using UnityEngine;

public class DroneAcroMovementAdjuster : IDroneFlightModeMovementAdjuster
{
    private float _leapAngleThreshold;
    float IDroneFlightModeMovementAdjuster.ActionAngleThreshold
    {
        set => _leapAngleThreshold = value;
    }

    Vector2 IDroneFlightModeMovementAdjuster.GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation)
    {
        return rawInputVector;
    }
}
