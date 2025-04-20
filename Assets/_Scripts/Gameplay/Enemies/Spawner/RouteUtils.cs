using System.Linq;
using UnityEngine;

public static class RouteUtils
{
    public static Vector3[][] GetConvertedRoutesWaypointsPositions(GameObject[] routesParents)
    {
        Vector3[][] routesWaypointsPositions = new Vector3[routesParents.Length][];
        for (int i = 0; i < routesParents.Length; i++)
        {
            Transform[] routeWaypointsTransforms = routesParents[i].
                GetComponentsInChildren<Transform>().
                Where(t => t != routesParents[i].transform).
                ToArray();
            routesWaypointsPositions[i] = new Vector3[routeWaypointsTransforms.Length];
            for (int j = 0; j < routeWaypointsTransforms.Length; j++)
            {
                routesWaypointsPositions[i][j] = routeWaypointsTransforms[j].position;
            }
        }

        return routesWaypointsPositions;
    }
}
