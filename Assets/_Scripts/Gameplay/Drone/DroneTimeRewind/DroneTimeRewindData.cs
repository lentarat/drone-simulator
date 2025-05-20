using UnityEngine;

public struct DroneTimeRewindData
{
    public Vector3 Position { get; private set; }
    public Vector3 RigidbodyVelocity { get; private set; }
    public Quaternion Rotation { get; private set; }
    public DroneTimeRewindData(Vector3 position, Vector3 rigidbodyVelocity, Quaternion rotation)
    {
        Position = position;
        RigidbodyVelocity = rigidbodyVelocity;
        Rotation = rotation;
    }
}
