using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackPlayer : IStage
{
    public Transform Target;
    public EnemyTrackPlayer(Transform transform)
    {
        Target = transform;
    }
    public Vector3? GetLocation()
    {
        return Target.position;
    }
}
