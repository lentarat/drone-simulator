using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Target : MonoBehaviour
{
    [SerializeField] private TargetRoutesSO _routes;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private int _waypointPositionIndex;
    private Vector3[] _routeWaypointsPositions;
     
    public void Init(Vector3[] routeWaypointsPositions, int waypointPositionIndex )
    {
        _routeWaypointsPositions = routeWaypointsPositions;
        _waypointPositionIndex = waypointPositionIndex;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        StartRoute();
    }

    private void StartRoute()
    { 
        //_navMeshAgent.
    }
}
