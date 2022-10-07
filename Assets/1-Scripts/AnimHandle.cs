using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class AnimHandle : MonoBehaviour, IAnimationController
{
    public Animator charAnimatorRef;
    public SpriteRenderer enemyMaterial;

    private void Start()
    {

    }

    public void SetCharAnimSpeed(float speed)
    {
        if (charAnimatorRef != null)
        {
            charAnimatorRef.SetFloat("Speed", speed);
        }
    }

    public void ReciveDamageAnim(Vector2 dir)
    {
        charAnimatorRef.SetFloat("ReciveDamageX", dir.x);
        charAnimatorRef.SetFloat("ReciveDamageY", dir.y);
        charAnimatorRef.SetTrigger("ReciveDamage");
    }

    public void SetAnimatorVec(Vector2 DirVec)
    {
        if (charAnimatorRef != null)
        {
            charAnimatorRef.SetFloat("Hortizanal", DirVec.y);
            charAnimatorRef.SetFloat("Vertical", DirVec.x);
        }
    }
    public void SetDeath()
    {
        charAnimatorRef.SetBool("Killed",true);
        enemyMaterial.sortingOrder = -4;
    }

    public void SetDraw()
    {
        charAnimatorRef.SetTrigger("Draw");
    }
    public void SetRelease()
    {
        charAnimatorRef.SetTrigger("Release");
    }
    public void SetAttackDir(Vector2 attackDir)
    {
        charAnimatorRef.SetFloat("AttackDirX", attackDir.x);
        charAnimatorRef.SetFloat("AttackDirY", attackDir.y);
    }

    public void SetAttackDir(Vector2 attackDir,AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Strike:
                charAnimatorRef.SetBool("Strike", true);
                break;
            case AttackType.DoubleStrike:
                charAnimatorRef.SetBool("DoubleStrike", true);
                break;
            case AttackType.TripleStrike:
                break;
            default:
                break;
        }
        charAnimatorRef.SetFloat("AttackDirX", attackDir.x);
        charAnimatorRef.SetFloat("AttackDirY", attackDir.y);
        charAnimatorRef.SetTrigger("Attack");
    }

    public void SetAttackDir(Vector2 attackDir, HoplomachusWeaponAttackType attackType)
    {
        switch (attackType)
        {
            case HoplomachusWeaponAttackType.Spear:
                charAnimatorRef.SetTrigger("Spear");
                break;
            case HoplomachusWeaponAttackType.SmallSword:
                charAnimatorRef.SetTrigger("SmallSword");
                break;
            default:
                break;
        }
        charAnimatorRef.SetFloat("AttackDirX", attackDir.x);
        charAnimatorRef.SetFloat("AttackDirY", attackDir.y);
        
    }

    public void SetStun(bool isStun)
    {
        charAnimatorRef.SetBool("Stun",isStun);

    }

    public Material GetMaterial()
    {
        return enemyMaterial.material;
    }

    public void SetAnimSpeedIdle(float Mult)
    {
        charAnimatorRef.SetFloat("AnimSpeedMult", Mult);
    }

    public void SetAttackSpeed(string Paramtere, float AttackSpeed, Animator WeaponAnim)
    {
        charAnimatorRef.SetFloat(Paramtere,AttackSpeed);
        WeaponAnim.SetFloat(Paramtere, AttackSpeed); ;
    }

    public void SetReciveDamageMulti(float multi)
    {
        charAnimatorRef.SetFloat("ReciveDamageMulti", 1 / multi);

    }
}
