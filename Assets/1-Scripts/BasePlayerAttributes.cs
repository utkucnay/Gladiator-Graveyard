using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentKits", menuName = "PlayerAttributes/Base Attributes", order = 1)]

public class BasePlayerAttributes : ScriptableObject
{
    public float str = 100;
    public float vit = 100;
    public float stam = 100;
    public float dex = 100;
    public float spd = 100;
}
