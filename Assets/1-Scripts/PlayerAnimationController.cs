using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class PlayerAnimationController : Singleton<PlayerAnimationController>, IAnimationController
{
    public Animator charAnimatorRef;
    public SpriteRenderer charMaterial;

    private void Start()
    {
        GameController.Instance.playerDied.AddListener(SetDeath);
        GameController.Instance.playerBorn.AddListener(SetBorn);

    }

    public void SetCharAnimSpeed(float speed)
    {
        if (charAnimatorRef != null)
        {
            charAnimatorRef.SetFloat("Speed", speed);
        }
    }

    public void ReciveDamageAnimBool(bool isReciveDamage)
    {
        charAnimatorRef.SetBool("ReciveDamage", isReciveDamage);
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
        charAnimatorRef.SetTrigger("Killed");
        charMaterial.sortingOrder = -4;
    }

    public void SetBorn()
    {
        
        charMaterial.sortingOrder = 0;
    }

    public Material GetMaterial()
    {
        return charMaterial.material;
    }

    public void SetParryDir(Vector2 DirVec)
    {
        if (charAnimatorRef != null)
        {
            charAnimatorRef.SetFloat("ParryDirX", DirVec.x);
            charAnimatorRef.SetFloat("ParryDirY", DirVec.y);
            charAnimatorRef.SetTrigger("Parry");
        }
    }
}
