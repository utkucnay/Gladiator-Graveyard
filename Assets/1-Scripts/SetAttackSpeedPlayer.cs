using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackSpeedPlayer : Singleton<SetAttackSpeedPlayer>
{
    public float PlayerAttackSpeed;
    [SerializeField]Animator anim;

    public BasePlayerAttributes playerAttributes;

    private void Start()
    {
        anim.SetFloat("AttackSpeed", PlayerAttackSpeed);
        PlayerAnimationController.Instance.charAnimatorRef.SetFloat("AttackSpeed", PlayerAttackSpeed);
    }
    private void OnEnable()
    {
        anim.SetFloat("AttackSpeed", PlayerAttackSpeed);
        PlayerAnimationController.Instance.charAnimatorRef.SetFloat("AttackSpeed", PlayerAttackSpeed);
    }

    public void SetAttackSpeedOnData()
    {
        PlayerAttackSpeed = playerAttributes.dex;
    }
}
