using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WaypointCreater : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float WaypointRadius = 0.1f;
    public Color PointColor = Color.red;
    public List<Vector3> Waypoints = new List<Vector3>();
    public List<bool> CheckBox = new List<bool>();
    public float MinGroundLevel = 1.0f;

    public int GetNextIndex(int index)
    {
        if (index + 1 >= Waypoints.Count)
        {
            return 0;
        }

        return index + 1;
    }

    public void CreateWaypoint()
    {
        Waypoints.Add(Vector3.zero);
    }

    public void RemoveWaypointFromList(int index)
    {
        Waypoints.RemoveAt(index);
    }

    public void SetGroundLevel(float minGroundLevel)
    {
        if (Waypoints.Count > 0)
        {
            for (var index = 0; index < Waypoints.Count; index++)
            {
                Waypoints[index] = new Vector3(Waypoints[index].x, minGroundLevel, Waypoints[index].z);
            }
        }
    }
}
