using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentKits", menuName = "EquipmentKits/Murmillo Attributes", order = 1)]

public class MurmilloAttributes : ScriptableObject
{
    public float gladiusDamage;
    public float shieldStaminaDrain;
    public int murmilloCoverWalkSpeedRatio;
    public bool canDash;
    [Header("Stamina Consume")]
    public float lightAttackStamina;
}
