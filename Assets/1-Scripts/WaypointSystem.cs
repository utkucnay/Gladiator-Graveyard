using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaypointSystem
{
    List<Waypoint> Waypoints;

    public WaypointSystem(int i)
    {
        Waypoints = new List<Waypoint>();
    }


    public void AddWaypoint(Waypoint waypoint)
    {
        Waypoints.Add(waypoint);
    }

    public Waypoint? GetWaypoint()
    {
        if (Waypoints.Count != 0)
            return Waypoints[0];
        return null;
    }
    public void RemoveWaypoint()
    {
        if (Waypoints.Count != 0)
            Waypoints.Remove(Waypoints[0]);
    }
}
