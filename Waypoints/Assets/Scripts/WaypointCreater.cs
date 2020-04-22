using System;
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
    public bool IsSetGroundLevel = false;
    public bool IsInsertPoint = false;

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

    public void CreateWaypoint(Vector3 v3)
    {
        WaypointsCheckBox.Add(false);
        Waypoints.Add(v3);
    }


    public void InsertPoint(Vector3 v3)
    {
        Dictionary<int, float> originWaypoints = new Dictionary<int, float>();

        for (int index = 0; index < Waypoints.Count; index++)
        {
            float distance = Mathf.Abs(Vector3.Distance(Waypoints[index], v3));
            originWaypoints.Add(index, distance);
        }

        int[] closests = originWaypoints.OrderBy(op => op.Value).Take(2).Select(op => op.Key).ToArray();

        if ((closests[0] == 0 && closests[1] == Waypoints.Count - 1) || (closests[1] == 0 && closests[0] == Waypoints.Count - 1))
        {
            WaypointsCheckBox.Add(false);
            Waypoints.Add(v3);
        } 
        else
        {
            int smallestIndex = closests.Min();
            WaypointsCheckBox.Insert(smallestIndex +1, false);
            Waypoints.Insert(smallestIndex + 1, v3);
        }
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
            if (Mathf.Abs((Waypoints[index] - v3).sqrMagnitude) < 1.5f)
            {
                Waypoints.RemoveAt(index);
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
