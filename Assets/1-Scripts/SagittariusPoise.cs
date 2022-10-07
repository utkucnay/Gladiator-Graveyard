using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusPoise : CharacterPoise
{
    SagittariusCombat sagittariusCombatRef;

    protected override void Start()
    {
        sagittariusCombatRef = GetComponent<SagittariusCombat>();
        MaxPoise = sagittariusCombatRef.SagiData.maksPoise;
        PoiseAddbySecond = sagittariusCombatRef.SagiData.poiseAddbySecond;
        PoiseRecoverTime = sagittariusCombatRef.SagiData.poiseRecoverTime;
        base.Start();
        
    }
}
