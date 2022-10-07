using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Glory
{
    static int TotalGlory;
    static int tempGlory;
    public static void AddGlory(int Glory)
    {
        tempGlory += Glory; 
        DOTween.To(GetGlory, SetGlory, tempGlory,  1f).SetEase(Ease.OutCirc);
    }
    public static void RemoveGlory(int Glory)
    {
        tempGlory -= Glory;
        DOTween.To(GetGlory, SetGlory, tempGlory, 1f).SetEase(Ease.OutCirc);
    }
    public static int GetGlory()
    {
        return TotalGlory;
    }
    public static int GetActualGlory()
    {
        return tempGlory;
    }
    public static void SetGlory(int Glory)
    {
        TotalGlory = Glory;
    }
}
