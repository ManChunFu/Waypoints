using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointCreater : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float WaypointRadius = 0.1f;
    public Color PointColor = Color.red;
    public List<Vector3> Waypoints = new List<Vector3>();
    public List<bool> WaypointsCheckBox = new List<bool>();
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
        WaypointsCheckBox.Add(false);
        Waypoints.Add(Vector3.zero);
    }

    public void CreateWaypoing(Vector3 v3)
    {
        WaypointsCheckBox.Add(false);
        Waypoints.Add(v3);
    }

    public void RemoveWaypoint()
    {
        for (int index = Waypoints.Count -1; index >= 0; index--)
        {
            if (WaypointsCheckBox[index])
            {
                Waypoints.RemoveAt(index);
                WaypointsCheckBox.RemoveAt(index);
            }
        }
    }

    public void RemoveWaypoint(Vector3 v3)
    {
        for (int index = Waypoints.Count -1; index >= 0; index--)
        {
            if (Mathf.Abs((Waypoints[index] - v3).sqrMagnitude) < 0.5f)
            {
                Waypoints.Remove(v3);
                WaypointsCheckBox.RemoveAt(index);
            }
        }
    }

    public bool IsAnySelected()
    {
        return WaypointsCheckBox.Any(wc => wc);
    }

    public void RemoveAll()
    {
        Waypoints.Clear();
        WaypointsCheckBox.Clear();
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
