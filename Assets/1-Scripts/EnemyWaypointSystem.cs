using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyWaypointSystem : IStage, IWaypointSystem
{
    WaypointSystem WaypointSystem ;

    public EnemyWaypointSystem(Transform[] waypoints)
    {
        WaypointSystem = new WaypointSystem();
        SetWaypoint(waypoints);
    }

    public EnemyWaypointSystem(Vector3[] waypoints)
    {
        WaypointSystem = new WaypointSystem(waypoints.Length);
        SetWaypoint(waypoints);
    }

    public Vector3? GetLocation()
    {
        if (WaypointSystem.GetWaypoint() == null)
        {
            return null;
        }
        return WaypointSystem.GetWaypoint().Value.waypoint;
    }

    public void SetWaypoint(Transform[] waypoints)
    {
        //Bu iþlemler ctor ile yapýlcak
        foreach (var item in waypoints)
        {
            Waypoint waypoint = new Waypoint();
            waypoint.waypoint = item.position;
            WaypointSystem.AddWaypoint(waypoint);
        }
    }

    public void SetWaypoint(Vector3[] waypoints)
    {
        //Bu iþlemler ctor ile yapýlcak
        foreach (var item in waypoints)
        {
            Waypoint waypoint = new Waypoint();
            waypoint.waypoint = item;
            WaypointSystem.AddWaypoint(waypoint);
        }
    }

    public bool CheckWaypoint(Vector3 Position)
    {
        if (WaypointSystem.GetWaypoint() == null) return false;
        if ((Position - WaypointSystem.GetWaypoint().Value.waypoint).magnitude <= 0.5f)
        {
            RemoveWaypoint();
        }
        return true;
    }

    public void RemoveWaypoint()
    {
        WaypointSystem.RemoveWaypoint();
    }
}
