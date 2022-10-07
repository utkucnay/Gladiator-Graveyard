using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class EnemyHealth : CharacterHealth
{
    protected EnemyCombat enemyMeleeAttackRef;
    protected EnemyMovement enemyMovementRef;
    public CharacterPoise charPoiseRef;
    [HideInInspector]public bool IsDead;
    public EnemyScriptableObject EnemyData;
    Material material;
    public UnityEngine.Events.UnityEvent RDDelay;
    public GameObject PlayerCollider;
    public override void Start()
    {
        base.Start();
        enemyMovementRef = gameObject.GetComponent<EnemyMovement>();
        enemyMeleeAttackRef = gameObject.GetComponent<EnemyCombat>();
        charPoiseRef = GetComponent<CharacterPoise>();
        material = new Material(enemyMovementRef.animHandleRef.enemyMaterial.material);
        enemyMovementRef.animHandleRef.SetReciveDamageMulti(EnemyData.CharReciveDamageTimeMulti);
    }

    public override void ReceiveDamage(HitInfo hitInfo)
    {
        base.ReceiveDamage(hitInfo);
      //  ParticleEffectController.Instance.CharAnimReciveDamage(PreAnimCorRef, animationController.GetMaterial(), EnemyData.CharReciveDamageTimeMulti, EnemyData.CharReciveDamageColor , this);
        ParticleEffectController.Instance.OnHitColorChange(animationController.GetMaterial(),  EnemyData.CharReciveColorTime, EnemyData.CharReciveColorMultiplier, EnemyData.CharReciveDamageColor, this);
    }

    public virtual void KillSelf(HitInfo hitInfo)
    {
        enemyMovementRef.animHandleRef.SetDeath();
        PlayerCollider.SetActive(false);
        gameObject.transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        enemyMovementRef.animHandleRef.enemyMaterial.material.SetColor("_TintColor", Color.white);
        enemyMovementRef.animHandleRef.enemyMaterial.material = new Material(material);
        enemyMovementRef.StopAllCoroutines();
        enemyMovementRef.animHandleRef.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        enemyMovementRef.agent.radius = 0.1f;
        enemyMovementRef.enabled = false;
        charPoiseRef.StopAllCoroutines();
        charPoiseRef.enabled = false;
        enemyMeleeAttackRef.HideAllWeapon();
        enemyMeleeAttackRef.StopAllCoroutines();
        enemyMeleeAttackRef.enabled = false;
        GetComponentInChildren<PlayerDetector>().gameObject.SetActive(false);
        Glory.AddGlory(EnemyData.Glory);
        IsDead = true;
        Debug.Log("Death");
        WaveController.Instance.EnemyDeath();
        WaveController.Instance.NavmeshOrderStack.Push(enemyMovementRef.agent.avoidancePriority);
        StartCoroutine(CloseColliderWithSec(EnemyData.deathPushTime));
        Vector3 pushVector = Vector3.zero;
        enemyMovementRef.PushSelf(hitInfo, EnemyData.deathPushMultiplier, EnemyData.deathPushTime, EnemyData.deathPushEase, out pushVector);
        //push Vector gittiði loc orada spamle particule
        Vector3 dir = -gameObject.transform.position + hitInfo.attackWeaponPosition;
        ParticleEffectController.Instance.PlayBloodParticleDeath(transform, hitInfo.attackWeaponPosition);

        Debug.Log(pushVector);
        //Destroy(gameObject, 5);
    }

    IEnumerator CloseColliderWithSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        enemyMovementRef.agent.enabled = false;
    }
}
