using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float m_Speed = 8.0f;
    [SerializeField] private WaypointCreater m_Waypoint;
    [SerializeField] private float m_WaypointDwellTime = 1.0f;

    private int m_CurrentWaypointIndex = 0;
    private float m_TimeSinceArrivedAtWaypoint = Mathf.Infinity;

    private void Start()
    {
        if (m_Waypoint != null)
        {
            return;
        }

        if (m_Waypoint == null)
        {
            m_Waypoint = FindObjectOfType<WaypointCreater>();
            if (m_Waypoint == null)
            {
                throw new MissingReferenceException("Missing reference of WaypointCreater script.");
            }
        }
    }

    private void Update()
    {
        PatrolBehavior();
    }

    private void PatrolBehavior()
    {
        if (m_Waypoint.Waypoints.Any())
        {
            if (AtWaypoint())
            {
                m_TimeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }

            m_TimeSinceArrivedAtWaypoint += Time.deltaTime;

            if (m_TimeSinceArrivedAtWaypoint > m_WaypointDwellTime)
            {
                transform.LookAt(GetCurrentWaypoint());
                transform.position = Vector3.MoveTowards(transform.position, GetCurrentWaypoint(), m_Speed * Time.deltaTime);
            }
        }
    }

    private Vector3 GetCurrentWaypoint()
    {
        return m_Waypoint.Waypoints[m_CurrentWaypointIndex];
    }
    private bool AtWaypoint()
    {
        return transform.position == GetCurrentWaypoint();
    }

    private void CycleWaypoint()
    {
        m_CurrentWaypointIndex = m_Waypoint.GetNextIndex(m_CurrentWaypointIndex);
    }
}
