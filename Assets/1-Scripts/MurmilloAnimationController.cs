using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurmilloAnimationController : Singleton<MurmilloAnimationController>
{
    public Animator GladiusAnimator;
    public Animator BodyAnimator;
    public GameObject attackGladiusRoot;
    public GameObject idleGladiusRoot;
    public GameObject attackGladius;
    public GameObject OpenShild;
    public GameObject CoverShild;

    public AnimatorStateInfo currStateInfo;
    private int lightAttackCombo;
    private bool characterDead;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    public override void PlayerDied()
    {
        base.PlayerDied();

        characterDead = true;
        attackGladiusRoot.SetActive(false);
        idleGladiusRoot.SetActive(false);
        HideShield();
    }

    public override void PlayerBorn()
    {
        base.PlayerBorn();
        characterDead = false;
        attackGladiusRoot.SetActive(false);
        idleGladiusRoot.SetActive(true);
        BodyAnimator.ResetTrigger("Killed");
        BodyAnimator.SetTrigger("Reborn");

        OpenShield();
    }

    public void CoverShield()
    {
        OpenShild.SetActive(false);
        CoverShild.SetActive(true);
    }

    public void OpenShield()
    {
        OpenShild.SetActive(true);
        CoverShild.SetActive(false);
    }

    public void HideShield()
    {
        OpenShild.SetActive(false);
        CoverShild.SetActive(false);
    }

    public void SwapGladius(bool isIdle)
    {
        if (characterDead == false)
        {
            if (isIdle)
            {
                attackGladiusRoot.SetActive(false);
                idleGladiusRoot.SetActive(true);
            }
            else
            {
                attackGladiusRoot.SetActive(true);
                idleGladiusRoot.SetActive(false);
            }
        }
    }

    public void SetCurrentStateInfo(AnimatorStateInfo stateInfo)
    {
        currStateInfo = stateInfo;
        PlaySoundWithStateInfo(PlayerCharacterCombat.Instance.combatState);

        PlayerCharacterMovement.Instance.LockMovement(currStateInfo.length );
    }

    public void PlaySoundWithStateInfo(PlayerCombatState combatState)
    {
        switch (combatState)
        {
            case PlayerCombatState.Idle:
                break;
            case PlayerCombatState.LeftAttack:
                AudioController.Instance.PlayAudio(AudioType.MurmilloGladiusSwing, 1);
                break;
            case PlayerCombatState.rightAttack:
                AudioController.Instance.PlayAudio(AudioType.MurmilloGladiusSwing, 1.3f);
                break;
            case PlayerCombatState.thrustAttack:
                AudioController.Instance.PlayAudio(AudioType.MurmilloGladiusSwing, 1.6f);
                break;
            case PlayerCombatState.CoverShield:
                break;
            case PlayerCombatState.Parry:
                break;
            default:
                break;
        }
    }

    public void PlayLightAttack(Direction rotDir)
    {
        SwapGladius(false);
        SetAttackGladiusOrder(rotDir);
        GladiusAnimator.SetTrigger("GladiusLightAttack");
        BodyAnimator.SetTrigger("GladiusLightAttack");


        lightAttackCombo++;
    }

    private void SetAttackGladiusOrder(Direction directionToSetOrder)
    {
       if(directionToSetOrder == Direction.South || directionToSetOrder == Direction.SouthEast || directionToSetOrder == Direction.SouthWest)
        {
            attackGladius.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
       else
        {
            attackGladius.GetComponent<SpriteRenderer>().sortingOrder = -3;
        }
    }

    public void HideWeapons()
    {

    }
}
