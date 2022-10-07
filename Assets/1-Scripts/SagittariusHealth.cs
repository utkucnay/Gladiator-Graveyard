using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusHealth : EnemyHealth
{
    SagittariusScriptableObject SagiData;
    private void Awake()
    {
        enemyMovementRef = gameObject.GetComponent<EnemyMovement>();
        enemyMeleeAttackRef = gameObject.GetComponent<EnemyCombat>();
        SagiData = (enemyMeleeAttackRef as SagittariusCombat).SagiData;
        maxHealth = SagiData.maxHealth;
        EnemyData = SagiData;
    }

    public override void ReceiveDamage(HitInfo hitInfo)
    {
        if ((enemyMeleeAttackRef as SagittariusCombat).amIDrawBow)
        {
            charPoiseRef.ReducePoise(hitInfo.damage * 0.5f);
        }
        else
        {
            charPoiseRef.ReducePoise(hitInfo.damage);
            enemyMovementRef.PushSelf(hitInfo, SagiData.pushMultiplier, SagiData.pushTime, SagiData.pushEase);
            enemyMovementRef.ReciveDamageAnim(hitInfo);
        }

        //update receive damage calculation
        base.ReceiveDamage(hitInfo);
        //enemyMovementRef.CharAnimReciveDamage(hitInfo);

       

        enemyMeleeAttackRef.SetAttackStun(0);
        Vector3 dir = gameObject.transform.position - hitInfo.attackWeaponPosition;

        switch (hitInfo.combatState)
        {
            case (PlayerCombatState.LeftAttack):
                {
                    ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, gameObject.transform.position + (Vector3)Vector2.Perpendicular(dir) * 3);
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, UnityEngine.Random.Range(1, 1.4f));
                    break;
                }
            case (PlayerCombatState.rightAttack):
                {
                    ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, gameObject.transform.position + -(Vector3)Vector2.Perpendicular(dir) * 3);
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, UnityEngine.Random.Range(1, 1.4f));
                    break;
                }
            case (PlayerCombatState.thrustAttack):
                {
                    ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, gameObject.transform.position + dir * 3);
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, UnityEngine.Random.Range(1.3f, 1.7f));
                    break;
                }
        }


        // kill here
        if (currentHealth <= 0)
        {
            KillSelf(hitInfo);
        }
    }

    public override void KillSelf(HitInfo hitInfo)
    {
        GetComponent<SagittariusAI>().StopAllCoroutines();
        base.KillSelf(hitInfo);
    }

    public override void Start()
    {
        base.Start();
        SagiData = (enemyMeleeAttackRef as SagittariusCombat).SagiData;
    }

   
}
