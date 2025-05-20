using UnityEngine;

public interface IDroneFlightModeMovementAdjuster
{
    float ActionAngleThreshold { set; }
    Vector2 GetAdjustedPitchAndRollInputVector(Vector2 rawInputVector, Quaternion droneRotation);
}
