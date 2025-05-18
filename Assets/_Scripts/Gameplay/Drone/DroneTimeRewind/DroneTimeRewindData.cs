using UnityEngine;

public struct DroneTimeRewindData
{
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public DroneTimeRewindData(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
