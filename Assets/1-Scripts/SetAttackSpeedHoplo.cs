using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackSpeedHoplo : MonoBehaviour
{
    public HoplomachusScriptableObject hoploData;
    public Animator anim;
    private void OnEnable()
    {
        anim.SetFloat("SmallSwordAttackSpeed", hoploData.SmallSwordAttackSpeed);
        anim.SetFloat("SpearAttackSpeed", hoploData.SpearAttackSpeed);
    }
}
