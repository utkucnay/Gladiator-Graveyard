using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusCombat : EnemyCombat
{
    public SagittariusScriptableObject SagiData;

    [SerializeField] private Transform attackRangePivot;
    
    [HideInInspector]public bool amIDrawBow;
    private Transform target;

    [SerializeField]private int ShrinkEffectID;
    private void Awake()
    {
        enemyMovementRef.Speed = SagiData.Speed;
    }
    private void Start()
    {
        enemyMovementRef = GetComponent<EnemyMovement>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyMovementRef.WeaponDraw.AddListener(ShowAllWeapon);
        
    }
    public new void HandleHitOnEnemy(GameObject hitObject, Collider2D collider)
    {
        //float damage = Damage;
        Vector3 dir = (hitObject.transform.position - attackRangePivot.transform.position).normalized;
        HitInfo hitInfo = new HitInfo(SagiData.Damage, SagiData.PushAmount, SagiData.ReduceStamina, dir, attackRangePivot.transform.position, PlayerCombatState.LeftAttack,HitType.Arrow);
        hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
    }

    public override void PlayAttack(Vector3 attackDir, string AnimParametre)
    {
        base.PlayAttack(attackDir, AnimParametre);
        enemyMovementRef.animHandleRef.SetAttackDir(attackDir);
        enemyMovementRef.animHandleRef.SetDraw();
    }

    public void AttackTarget(AIAction actionRef)
    {
        if (IsTargetInRange() && !AttackLock && attackStunTime <= 0)
        {
            AttackLock = true;
            float time = UnityEngine.Random.Range(SagiData.MinAttackDelay, SagiData.MaxAttackDelay);
            currentActionRef = actionRef;
            StartCoroutine(ParticleEffectController.Instance.ArrowReleaseSqueeze(enemyMovementRef.animHandleRef.charAnimatorRef.gameObject.transform, SagiData.shrinkEffects[ShrinkEffectID].shrinkEffectX,
            SagiData.shrinkEffects[ShrinkEffectID].shrinkEffectY, time * SagiData.shrinkEffects[ShrinkEffectID].ShrinkEffectTimeMulti));
            StartCoroutine(AttackBeginDelay(time));
        }
        else
        {
            actionRef.TriggerOncomplete(ActionResult.Failure);
        }
    }

    IEnumerator<WaitForSeconds> AttackBeginDelay(float time)
    {
        yield return new WaitForSeconds(0.1f);
        if (IsTargetInRange() && attackStunTime <= 0)
        {
            var AttacDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
            amIDrawBow = true;
            PlayAttack(AttacDir, "Draw");
            float passingTime = 0;

            while (passingTime < time)
            {
                yield return null;
                passingTime += Time.deltaTime;
                AttacDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
                LookAtTarget(AttacDir);
                enemyMovementRef.animHandleRef.SetAttackDir(AttacDir);
            }

            ReleaseArrow(AttacDir);
            amIDrawBow = false;
        }
        else
        {
            currentActionRef.TriggerOncomplete(ActionResult.Failure);
        }
    }

    void ReleaseArrow(Vector2 attackDir)
    {
        AudioController.Instance.PlayAudio(AudioType.BowRelease);

        float angle = Vector2.SignedAngle(new Vector2(1, 0), attackDir);
        var ArrowObject = Instantiate(SagiData.Arrow, EnemyAnimator.gameObject.transform.position, Quaternion.identity);
        ArrowObject.transform.eulerAngles += new Vector3(0, 0, angle);
        ArrowObject.GetComponent<Projectile>().SetDirection(attackDir);
        ArrowObject.GetComponent<Rigidbody2D>().AddForce(attackDir*25* SagiData.ArrowSpeedMulti);
        ArrowObject.GetComponent<AttackTriggerNotifier>().AddEvent(HandleHitOnEnemy);
        EnemyAnimator.SetTrigger("Release");
        enemyMovementRef.animHandleRef.SetRelease();
        enemyMovementRef.ChangePrevDir(enemyMovementRef.GetTarget().position - transform.position);
    }
    private bool IsTargetInRange()
    {
        if (SagiData.AttackRange >= Vector2.Distance(target.transform.position, attackRangePivot.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
