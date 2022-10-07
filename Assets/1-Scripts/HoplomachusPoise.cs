using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoplomachusPoise : CharacterPoise
{
    HoplomachusCombat HoplomachusCombatRef;


    protected override void Start()
    {
        HoplomachusCombatRef = GetComponent<HoplomachusCombat>();
        MaxPoise = HoplomachusCombatRef.HoploData.maksPoise;
        PoiseAddbySecond = HoplomachusCombatRef.HoploData.poiseAddbySecond;
        PoiseRecoverTime = HoplomachusCombatRef.HoploData.poiseRecoverTime;
        base.Start();
    }

    protected override void Update()
    {
        if (CurrentPoise < 0 && !IsPoise)
        {
            HoplomachusCombatRef.EndDefendAction();
        }
        base.Update();
    }
}
