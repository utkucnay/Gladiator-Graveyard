using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ShrinkEffects
{
    public float shrinkEffectX;
    public float shrinkEffectY;
    public float ShrinkEffectTimeMulti;
}
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Health")]
    public float maxHealth;

    [Header("Poise")]
    public float poiseAddbySecond;
    public int maksPoise;
    public int poiseRecoverTime;

    [Header("Combat")]
    public int Damage;
    public float PushAmount;
    public float ReduceStamina;
    public float MaxAttackDelay;
    public float MinAttackDelay;
    public float AttackSpeed;
    public float AttackDashRatio;
    public float MinAttackDash;
    public float MaxAttackDash;

    [Header("Movement")]
    public float Speed;
    public float LookDelay;

    [Header("Glory")]
    public int Glory;

    [Header("Effect")]
    public ShrinkEffects[] shrinkEffects;
    public Color CharReciveDamageColor;
    public float CharReciveDamageTimeMulti;
    public float CharReciveColorTime;
    public float CharReciveColorMultiplier;
    public float pushMultiplier;
    public float pushTime;
    public DG.Tweening.Ease pushEase;
    public float deathPushMultiplier;
    public float deathPushTime;
    public DG.Tweening.Ease deathPushEase;
}
