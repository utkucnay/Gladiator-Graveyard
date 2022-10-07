using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoplomachusHealth : EnemyHealth
{
    private CharacterRotator2D characterRotRef;
    HoplomachusCombat hoplomachusCombat;

    HoplomachusScriptableObject HoploData;
    private Color multiplyColor = new Color(1, 1, 1, 0);
    private void Awake()
    {
        hoplomachusCombat = gameObject.GetComponent<HoplomachusCombat>();
        characterRotRef = gameObject.GetComponent<CharacterRotator2D>();
        maxHealth = hoplomachusCombat.HoploData.maxHealth;
        multiplyColor *= hoplomachusCombat.HoploData.ShieldReciveDamageColor;
        EnemyData = hoplomachusCombat.HoploData;
        HoploData = hoplomachusCombat.HoploData;
    }

    public override void ReceiveDamage(HitInfo hitInfo)
    {
        Debug.Log(hoplomachusCombat.defending);
        if (!hoplomachusCombat.amIDrawSpear && !hoplomachusCombat.attacking && !hoplomachusCombat.defending)
        {
            RDDelay.Invoke();
        }
        if (hoplomachusCombat.defending)
        {

            float defenceAngle = Vector3.Angle(characterRotRef.currentDirectionVector, hitInfo.hitDirection);
            if (defenceAngle < 120)
            {
                ReciveDmg(hitInfo);
                Debug.Log("in 120");
            }
            else
            {
                AudioController.Instance.PlayAudio(AudioType.ShieldHit, 1 + (1 - charPoiseRef.CurrentPoise / charPoiseRef.MaxPoise) * 0.8f);
                //shield protect
                //enemyMovementRef.PushSelf(hitInfo, 0.05f, 0.1f);
                enemyMovementRef.PushSelf(hitInfo, HoploData.pushShieldMultiplier, HoploData.pushTime, HoploData.pushEase);
                charPoiseRef.ReducePoise(hitInfo.damage * 0.5f);
                switch (enemyMovementRef.Cr.currentDirection)
                {
                    case Direction.North:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northObjectOrders[1].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
                        break;
                    case Direction.NorthEast:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northEastObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
                        break;
                    case Direction.SouthEast:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southEastObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
                        break;
                    case Direction.South:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
                        break;
                    case Direction.SouthWest:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.southWestObjectOrders[2].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
                        break;
                    case Direction.NorthWest:
                        ParticleEffectController.Instance.ShieldAnimReciveDamage(PreAnimCorRef, enemyMovementRef.Cr.northWestObjectOrders[1].Object.GetComponent<SpriteRenderer>().material,
                            HoploData.ShieldEffectTime, HoploData.ShieldReciveDamageColor + multiplyColor * HoploData.ShieldColorNorthMult);
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
        if (hoplomachusCombat.attacking)
        {
            charPoiseRef.ReducePoise(hitInfo.damage * 0.5f);
        }
        else
        {
            charPoiseRef.ReducePoise(hitInfo.damage);
            enemyMovementRef.ReciveDamageAnim(hitInfo);
            enemyMovementRef.PushSelf(hitInfo, HoploData.pushMultiplier, HoploData.pushTime, HoploData.pushEase);
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
    }

    public override void KillSelf(HitInfo hitInfo)
    {
        hoplomachusCombat.HideShield();
        GetComponent<HoplomachusAI>().StopAllCoroutines();
        GetComponent<HoplomachusAI>().enabled = false;
        hoplomachusCombat.HideShield();
        base.KillSelf(hitInfo);

    }

    public override void Start()
    {
        base.Start();
        hoplomachusCombat = GetComponent<HoplomachusCombat>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
