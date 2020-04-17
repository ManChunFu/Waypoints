using System.Collections.Generic;
using UnityEngine;

public class WaypointCreater : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float WaypointGizmoRadius = 0.1f;
    public Color GizmoColor = Color.red;
    public List<Vector3> Waypoints = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if (Waypoints.Count > 0)
        {
            for (int indexPoint = 0; indexPoint < Waypoints.Count; indexPoint++)
            {
                int nextPoint = GetNextIndex(indexPoint);
                Gizmos.DrawSphere(GetWaypoint(indexPoint), WaypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(indexPoint), GetWaypoint(nextPoint));
            }
        }
    }

    public int GetNextIndex(int index)
    {
        if (index + 1 >= Waypoints.Count)
        {
            return 0;
        }

        return index + 1;
    }

    public Vector3 GetWaypoint(int index)
    {
        return Waypoints[index];
    }
}
