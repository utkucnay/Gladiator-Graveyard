using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThraexPoise : CharacterPoise
{
    ThraexCombat thraexCombatRef;

    protected override void Start()
    {
        thraexCombatRef = GetComponent<ThraexCombat>();
        MaxPoise = thraexCombatRef.ThraexData.maksPoise;
        PoiseAddbySecond = thraexCombatRef.ThraexData.poiseAddbySecond;
        PoiseRecoverTime = thraexCombatRef.ThraexData.poiseRecoverTime;
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ReducePoise(float number)
    {
        base.ReducePoise(number);
        if (CurrentPoise <= 0 && !IsPoise )
        {
            thraexCombatRef.EndDefendAction();
        }
    }
}
