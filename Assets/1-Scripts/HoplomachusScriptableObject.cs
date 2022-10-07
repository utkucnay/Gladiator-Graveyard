using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HoploData", menuName = "EnemyDatas/HoploData", order = 1)]
public class HoplomachusScriptableObject : EnemyScriptableObject
{
    [Header("Small Sword")]
    [Header("Hoplo Combat")]
    public int SmallSwordDamage;
    public float SmallSwordPushAmount;
    public float SmallSwordReduceStamina;
    public float SmallSwordMaxAttackDelay;
    public float SmallSwordMinAttackDelay;
    public float SmallSwordAttackSpeed;
    public float SmallSwordAttackDashRatio;
    public float SmallSwordMinAttackDash;
    public float SmallSwordMaxAttackDash;

    [Header("Throw Spear")]
    public int ThrowSpearDamage;
    public float ThrowSpearPushAmount;
    public float ThrowSpearReduceStamina;
    public float ThrowSpearMaxAttackDelay;
    public float ThrowSpearMinAttackDelay;
    public float ThrowSpearSpeedMulti;
    public GameObject Spear;

    [Header("Spear")]
    public int SpearDamage;
    public float SpearPushAmount;
    public float SpearReduceStamina;
    public float SpearMaxAttackDelay;
    public float SpearMinAttackDelay;
    public float SpearAttackSpeed;
    public float SpearAttackDashRatio;
    public float SpearMinAttackDash;
    public float SpearMaxAttackDash;
    public float SpearAttackDashTimeMulti;

    [Header("Hoplo Effect")]
    public float ShieldEffectTime;
    public float ShieldColorSouthMult;
    public float ShieldColorNorthMult;
    public Color ShieldReciveDamageColor;
    public float pushShieldMultiplier;
    public float ShieldOpenIdleAnimSpeed;

    [Header("Player Close")]
    [Header("Hoplo AI")]
    public int DefendChance;
    public float minDefTime;
    public float maxDefTime;
    [Space]
    public int AttackChance;

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


    [Header("Throw Spear Event")]
    public int ThrowSpearChance;
    public float ThrowSpearAngle;
    public float ThrowSpearFailMinTime;
    public float ThrowSpearFailMaxTime;

}
