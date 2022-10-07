using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
public class ThreaxAICombat : MonoBehaviour , IEnemyHitter
{
    enum AttackType
    {
        Strike,
        DoubleStrike,
        TripleStrike
    }

    public Transform AttackRangePivot;
    public bool Death;
   
    [SerializeField]
    private float TargetDistance;
    [Header("Attack Delay")]
    [SerializeField] private float maxAttackDelay;
    [SerializeField] private float minAttackDelay;
    private Transform Target;
    private EnemyMovement enemyMovementRef;
    public EnemyCombat enemyMeleeAttackRef;
    private CharacterRotator2D characterRotRef;
    [SerializeField]
    private float attackMovementRatio = 0.5f;
    private Coroutine PreCharAnimReciveDamageCorRef;
    public AnimHandle animHandle;
    public float ReciveDmageAndDashTime;

    void Start()
    {
        enemyMovementRef = GetComponent<EnemyMovement>();
        Target = enemyMovementRef.GetTarget();
        characterRotRef = gameObject.GetComponent<CharacterRotator2D>();
        enemyMeleeAttackRef.SwapWeapon(true);
    } 

    void Update()
    {
        //LockWithDistance();
        //AttackTarget();
        //tükürükle yamaladým düzeltilcek
        
    }

    private bool isTargetRange()
    {
        if (TargetDistance >= Vector2.Distance(Target.transform.position, AttackRangePivot.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HandleHitOnEnemy(GameObject hitObject, Collider2D collider)
    {
        float damage = 20;
        Vector3 dir = (hitObject.transform.position - AttackRangePivot.transform.position).normalized;
        HitInfo hitInfo = new HitInfo(damage, 2, 20, dir,AttackRangePivot.transform.position, PlayerCombatState.LeftAttack, HitType.MeleeDefault);
        hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
    }

    public void PushSelfAttack()
    {
        var AttacDir = Target.position - AttackRangePivot.transform.position;
        var NormDir = AttacDir.normalized;
        enemyMovementRef.PushSelfAttack(NormDir, AttacDir.magnitude * attackMovementRatio);
    }

}
