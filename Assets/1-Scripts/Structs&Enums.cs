using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitInfo
{
    public float damage;
    public float pushAmount;
    public float staminaConsumeOnShieldHit;
    public Vector3 hitDirection;
    public Vector3 attackWeaponPosition;
    public PlayerCombatState combatState;
    public HitType hitType;

  
    public HitInfo(float damage, float pushAmount, float staminaConsumeOnShieldHit, Vector3 hitDirection, Vector3 attackWeaponPosition, PlayerCombatState combatState, HitType hitType)
    {
        this.damage = damage;
        this.pushAmount = pushAmount;
        this.staminaConsumeOnShieldHit = staminaConsumeOnShieldHit;
        this.hitDirection = hitDirection;
        this.attackWeaponPosition = attackWeaponPosition;
        this.combatState = combatState;
        this.hitType = hitType;
    }
}
public struct Waypoint
{
    public Vector3 waypoint;
    public float Speed;
    public float Acceleration;
    public int Order;
}
public enum ActionResult
{
    Success,
    Failure
}
public enum EnemyCombatState
{
   Idle,
   Attacking,
   Defending,
   Moving
}
public enum HitType
{
    Arrow,
    SpearRanged,
    MeleeSword,
    MeleeDefault
}
public struct CardInfo
{
    public float cost;
    public string title;
    public string details;
    public Sprite cardImage;
}
public enum CardType
{
    Dexterity,
    Vitality,
    Strength,
    Speed,
    Stamina,
    SpikyShield,
    StrongParry,
    RegenerativeParry,
    TransformationOfPower,
    FirmGrip,
    BuildingAnger
}
public enum AttackType
{
    Strike,
    DoubleStrike,
    TripleStrike
}
public enum Direction
{
    North,
    NorthEast,
    SouthEast,
    South,
    SouthWest,
    NorthWest,
}
public enum EquipmentKits
{
    Murmillo,
    Dimachaerus,
    Sagittarius
}
public enum PlayerCombatState
{
    Idle,
    LeftAttack,
    rightAttack,
    thrustAttack,
    CoverShield,
    Parry
}

[Serializable]
public struct ObjectOrder
{
    public GameObject Object;
    public int Order;
}


