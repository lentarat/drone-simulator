using UnityEngine;

public class TargetRouteData
{
    public Vector3[] WaypointsPositions { get; private set; }
    public int WaypointPositionIndex { get; private set; }

    public TargetRouteData(Vector3[] waypointsPositions, int waypointPositionIndex)
    {
        WaypointsPositions = waypointsPositions;
        WaypointPositionIndex = waypointPositionIndex;
    }
}