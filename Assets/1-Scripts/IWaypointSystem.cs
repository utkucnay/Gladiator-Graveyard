using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaypointSystem
{
    void SetWaypoint(Transform[] waypoints);
    bool CheckWaypoint(Vector3 Position);
    void RemoveWaypoint();
}

