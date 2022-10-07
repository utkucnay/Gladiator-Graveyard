using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ThraexData", menuName = "EnemyDatas/ThraexData", order = 1)]
public class ThraexScriptableObject : EnemyScriptableObject
{
    [Header("Thraex Effect")]
    public float ShieldEffectTime;
    public float ShieldColorSouthMult; 
    public float ShieldColorNorthMult;
    public Color ShieldReciveDamageColor;
    public float pushShieldMultiplier;
    public float ShieldOpenIdleAnimSpeed;

    [Header("Player Close")]
    [Header("Thraex AI")]
    public int DefendChance;
    public float minDefTime;
    public float maxDefTime;
    [Space]
    public int AttackChance;
    [Space]
    public int WaitChance;
    public float minWaitTime;
    public float maxWaitTime;

    [Header("Far From Player")]
    public int MoveTowordsPlayerChance;
    public float minMoveTowordsPlayer;
    public float maxMoveTowordsPlayer;
    [Space]
    public int MoveWaypointChance;
    public int minWaypoint;
    public int maxWaypoint;
    public float longMin, longMax;
    public float shortMin, shortMax;
}
