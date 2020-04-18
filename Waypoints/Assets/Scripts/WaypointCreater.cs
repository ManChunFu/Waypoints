using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WaypointCreater : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float WaypointRadius = 0.1f;
    public Color PointColor = Color.red;
    public List<Vector3> Waypoints = new List<Vector3>();

    public int GetNextIndex(int index)
    {
        if (index + 1 >= Waypoints.Count)
            return 0;

        return index + 1;
    }

    public Vector3 GetWaypoint(int index)
    {
        return Waypoints[index];
    }

    public void CreateWaypoint()
    {
        //GameObject obj = new GameObject("point" + m_PointIndex.ToString());
        //obj.transform.position = Vector3.one;
        //obj.transform.SetParent(transform);
        Waypoints.Add(Vector3.zero);
        //m_PointIndex++;
    }

    public void RemoveWaypointFromList(int index)
    {
        Waypoints.RemoveAt(index);
    }
}
