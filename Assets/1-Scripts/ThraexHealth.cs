using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class ThraexHealth : EnemyHealth
{
    private CharacterRotator2D characterRotRef;
    ThraexCombat thraexCombat;
    private Color multiplyColor = new Color(1, 1, 1, 0);
    private ThraexScriptableObject ThraexData;



    private void Awake()
    {
        thraexCombat = gameObject.GetComponent<ThraexCombat>();
        characterRotRef = gameObject.GetComponent<CharacterRotator2D>();
        maxHealth = thraexCombat.ThraexData.maxHealth;
        multiplyColor *= thraexCombat.ThraexData.ShieldReciveDamageColor;
        EnemyData = thraexCombat.ThraexData;
        RDDelay = new UnityEngine.Events.UnityEvent();
    }

    public override void ReceiveDamage(HitInfo hitInfo)
    {
        Debug.Log(thraexCombat.defending);
        if (!thraexCombat.attacking && !thraexCombat.defending)
        {
            RDDelay.Invoke();
        }
        if (thraexCombat.defending)
        {

            float defenceAngle = Vector3.Angle(characterRotRef.currentDirectionVector, -hitInfo.hitDirection);
            if (defenceAngle > 80)
            {
                ReciveDmg(hitInfo);
                Debug.Log("in 120");
            }
            else
            {
                //shield protect
                AudioController.Instance.PlayAudio(AudioType.ShieldHit, 1 + (1 - charPoiseRef.CurrentPoise / charPoiseRef.MaxPoise) * 0.8f);

                enemyMovementRef.PushSelf(hitInfo,ThraexData.pushShieldMultiplier, ThraexData.pushTime, ThraexData.pushEase);
                charPoiseRef.ReducePoise(hitInfo.damage * 0.5f);
                
                FeelFeedbackController.Instance.PlayFeedback(FeelType.ShieldHitFeedback);

                ParticleSystem particle = Instantiate(Storage.Instance.shieldHitParticle,
                    transform.position, quaternion.identity);
                
                switch (enemyMovementRef.Cr.currentDirection)
                {
                    case Direction.North:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northObjectOrders[1].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor + multiplyColor * ThraexData.ShieldColorNorthMult);

                        particle.transform.position =
                            enemyMovementRef.Cr.northObjectOrders[1].Object.transform.position;
                        particle.Play();

                        break;
                    case Direction.NorthEast:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northEastObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor+ multiplyColor * ThraexData.ShieldColorNorthMult);
                        
                        particle.transform.position =
                            enemyMovementRef.Cr.northEastObjectOrders[2].Object.transform.position;
                        particle.Play();

                        break;
                    case Direction.SouthEast:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southEastObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor+ multiplyColor * ThraexData.ShieldColorSouthMult);
                        
                        particle.transform.position =
                            enemyMovementRef.Cr.southEastObjectOrders[2].Object.transform.position;
                        particle.Play();

                        break;
                    case Direction.South:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor+ multiplyColor * ThraexData.ShieldColorSouthMult);
                        
                        particle.transform.position =
                            enemyMovementRef.Cr.southObjectOrders[2].Object.transform.position;
                        particle.Play();

                        break;
                    case Direction.SouthWest:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southWestObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor+ multiplyColor * ThraexData.ShieldColorSouthMult);
                        
                        particle.transform.position =
                            enemyMovementRef.Cr.southWestObjectOrders[2].Object.transform.position;
                        particle.Play();

                        break;
                    case Direction.NorthWest:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northWestObjectOrders[1].Object.GetComponent<SpriteRenderer>().material,
                            ThraexData.ShieldEffectTime, ThraexData.ShieldReciveDamageColor+ multiplyColor * ThraexData.ShieldColorNorthMult);
                        
                        particle.transform.position =
                            enemyMovementRef.Cr.northWestObjectOrders[1].Object.transform.position;
                        
                        particle.Play();
                        break;
                    default:
                        break;
                }
                Debug.Log("out 120");
            }
        }
        else
        {
            ReciveDmg(hitInfo);


        }


        // kill here
        if (currentHealth <= 0)
        {
            KillSelf(hitInfo);
        }
    }

    public void ReciveDmg(HitInfo hitInfo)
    {

        if (thraexCombat.attacking)
        {
            charPoiseRef.ReducePoise(hitInfo.damage * 0.5f);
        }
        else
        {
            charPoiseRef.ReducePoise(hitInfo.damage);
            enemyMovementRef.PushSelf(hitInfo, ThraexData.pushMultiplier, ThraexData.pushTime, ThraexData.pushEase);
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
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, 1,1.4f);

                    break;
                }
            case (PlayerCombatState.rightAttack):
                {
                    ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, gameObject.transform.position + -(Vector3)Vector2.Perpendicular(dir) * 3);
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, 1, 1.4f);


                    break;
                }
            case (PlayerCombatState.thrustAttack):
                {
                    ParticleEffectController.Instance.PlayBloodParticle(gameObject.transform.position, gameObject.transform.position + dir * 3);
                    AudioController.Instance.PlayAudio(AudioType.FleshHit, 1.3f, 1.7f);


                    break;
                }
        }
    }

    public override void KillSelf(HitInfo hitInfo)
    {
        thraexCombat.HideShield();
        GetComponent<ThraexAI>().StopAllCoroutines();
        GetComponent<ThraexAI>().enabled = false;
        base.KillSelf(hitInfo);
        
    }

    public override void Start()
    {
        base.Start();
        thraexCombat = GetComponent<ThraexCombat>();
        ThraexData = thraexCombat.ThraexData;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
        base.Update();
        
    }
}
