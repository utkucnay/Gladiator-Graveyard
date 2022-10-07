using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackSpeed : MonoBehaviour
{
    public ThraexScriptableObject threaxData;
    public Animator anim;
    private void OnEnable()
    {
        anim.SetFloat("AttackSpeed",threaxData.AttackSpeed);
    }
}
