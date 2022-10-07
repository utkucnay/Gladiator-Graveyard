using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Poise
{
    public float CurrentPoise { get; private set; }
    public float PoiseAddbySecond { get; private set; }
    public int MaksPoise { get; private set; }
    public int PoiseRecoverTime { get; set; }
    public bool IsPoise { get; private set; }
    UnityEvent BreakPoise = new UnityEvent();

    public UnityAction<UnityAction> AddBreakPoiseEvent ;
    public UnityAction StartBreakPoiseEvent;
 
    public Poise(int maksPoise, float poiseAddbySecond , int poiseRecoverTime)
    {
        AddBreakPoiseEvent = Action => { BreakPoise.AddListener(Action); };
        StartBreakPoiseEvent = () => { if(BreakPoise != null) BreakPoise.Invoke(); };

        MaksPoise = maksPoise;
        CurrentPoise = maksPoise;
        PoiseAddbySecond = poiseAddbySecond;
        PoiseRecoverTime = poiseRecoverTime;
        IsPoise = false;
    }

    public void ReducePoise(float number)
    {
        CurrentPoise -= number;
        if (CurrentPoise <= 0 && !IsPoise)
        {
            StartBreakPoiseEvent();
            IsPoise = true;
        }
    }
    
    public void AddPoisebyTime(float deltaTime)
    {
        CurrentPoise += deltaTime * PoiseAddbySecond;
        if (CurrentPoise > MaksPoise)
        {
            CurrentPoise = MaksPoise;
        }
    }

    public void ResetPoise(AIAction action,AnimHandle animHandleRef)
    {
        CurrentPoise = MaksPoise;
        IsPoise = false;
        animHandleRef.SetStun(false);
        action.TriggerOncomplete(ActionResult.Success);
    }
}
