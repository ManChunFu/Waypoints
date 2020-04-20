
using UnityEngine;

public class Waypoint
{
    public Vector3 Position;
    public bool IsSelected;
    public Waypoint(Vector3 position, bool isSelected = false)
    {
        Position = position;
        IsSelected = isSelected;
    }

}