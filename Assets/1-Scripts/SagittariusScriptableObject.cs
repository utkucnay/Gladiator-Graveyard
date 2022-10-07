using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SagiData", menuName = "EnemyDatas/SagiData", order = 1)]
public class SagittariusScriptableObject : EnemyScriptableObject
{
    [Header("Sagittarius Combat")]
    public GameObject Arrow;
    public float ArrowSpeedMulti;
    public float AttackRange;

    [Header("Far From Player")]
    [Header("Thraex AI")]
    public int AttackChance;
    [Space]
    public int MoveWaypointChance;
    public int minWaypoint;
    public int maxWaypoint;
    public float minRadius, maxRadius;
    public float minAngle, maxAngle;
    [Header("Run From Player Speed")]
    public float RunFromPlayerSpeed;
    public float RunFromPlayerDrawSpeed;
}
