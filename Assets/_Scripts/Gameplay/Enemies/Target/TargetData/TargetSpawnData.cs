using UnityEngine;

public class TargetSpawnData
{
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Transform Parent { get; private set; }

    public TargetSpawnData(Vector3 position, Quaternion rotation, Transform parent)
    {
        Position = position;
        Rotation = rotation;
        Parent = parent;
    }
}

