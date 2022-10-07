using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThraexCombat : EnemyCombat
{
    [Header("Thraex Speacial")]
    public ThraexScriptableObject ThraexData;

    [SerializeField] private Transform attackRangePivot;
    [SerializeField] private GameObject shieldUp;
    [SerializeField] private GameObject shieldDown;

    [HideInInspector] public bool defending;
    [HideInInspector] public bool attacking;

    private Queue<Vector3> playerPositions;
    private bool lookAtTarget;
    private Transform target;

    private void Awake()
    {
        enemyMovementRef.Speed = ThraexData.Speed;
    }

    void Start()
    {
        playerPositions = new Queue<Vector3>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //enemyMovementRef.animHandleRef.SetAttackSpeed("AttackSpeed", ThraexData.AttackSpeed, EnemyAnimator);
        SwapWeapon(true);
        enemyHealthRef = GetComponent<EnemyHealth>();
        //attack speed kodu yazýlcak
        enemyMovementRef.WeaponDraw.AddListener(ShowAllWeapon);
        enemyMovementRef.WeaponDraw.AddListener(ShowAllShield);
    }

    void Update()
    {
        if (timerEnabled)
        {
            currentTime += Time.deltaTime;
            playerPositions.Enqueue(target.position);
            
        }

        if(lookAtTarget && !enemyHealthRef.IsDead)
        {
            if (playerPositions.Count != 0)
            {
                Vector3 dir = (playerPositions.Dequeue() - gameObject.transform.position).normalized;
                enemyMovementRef.AIRotateToDir(dir);
            }
        }

        if (currentTime > timer)
        {
            EndDefendAction();
            if (currentActionRef != null)
            {
                currentActionRef.TriggerOncomplete(ActionResult.Success);
            }
        }
    }

    public void EndDefendAction()
    {
        currentTime = 0;
        timerEnabled = false;
        defending = false;
        SwitchDefendMode(false);
        enemyMovementRef.animHandleRef.SetAnimSpeedIdle(1f);
    }

    public void DefendWithShield(AIAction actionRef, float timeToDefend)
    {
        currentActionRef = actionRef;
        timer = timeToDefend;
        currentTime = 0;
        timerEnabled = true;
        defending = true;
        SwitchDefendMode(true);
        enemyMovementRef.animHandleRef.SetAnimSpeedIdle(ThraexData.ShieldOpenIdleAnimSpeed);
    }

    public void ShowAllShield()
    {
        shieldDown.transform.parent.gameObject.SetActive(true);
    }

    public override void TriggerActionFailure(AIAction action)
    {
        if (currentActionRef != null)
        {
            EndDefendAction();
            attacking = false;
            StopAllCoroutines();
        }
        base.TriggerActionFailure(action);
    }

    public void AttackTarget(AIAction actionRef)
    {
        if (!AttackLock)
        {
            AttackLock = true;
            float time = Random.Range(ThraexData.MinAttackDelay, ThraexData.MaxAttackDelay);
            currentActionRef = actionRef;
            StartCoroutine(AttackBeginDelay(time));
        }
        else
        {
            actionRef.TriggerOncomplete(ActionResult.Failure);
        }
    }

    IEnumerator AttackBeginDelay(float time)
    {
        yield return new WaitForSeconds(time);
        attacking = true;
        AttackType RandStrikeType = (AttackType)Random.Range(0, 3);
        var AttacDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
        PlayAttack(AttacDir, RandStrikeType.ToString());
        //MoveTowardsAttack(AttacDir, 1);
    }

    public override void TriggerActionComplete()
    {
        if (currentActionRef != null)
        {
            currentActionRef.TriggerOncomplete(ActionResult.Success);
            currentActionRef = null;
            AttackLock = false;
            attacking = false;
        }
    }



    public override void HandleHitOnEnemy(GameObject hitObject, Collider2D collider)
    {
        if (PlayerCharacterCombat.Instance.parry)
        {
            base.Parry();
        }
        else
        {
            Vector3 dir = (hitObject.transform.position - attackRangePivot.transform.position).normalized;
            HitInfo hitInfo = new HitInfo(ThraexData.Damage, ThraexData.PushAmount, ThraexData.ReduceStamina, dir, attackRangePivot.transform.position, PlayerCombatState.LeftAttack, HitType.MeleeDefault);
            hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
        }
    }

    public void SwitchDefendMode(bool defending)
    {
        if (defending)
        {
            shieldUp.SetActive(true);
            shieldDown.SetActive(false);
            defending = true;
            playerPositions.Clear();
            StartCoroutine(LookAtWithDelay(ThraexData.LookDelay));
        }
        else
        {
            shieldUp.SetActive(false);
            shieldDown.SetActive(true);
            defending = false;
            playerPositions.Clear();
            lookAtTarget = false;
        }
    }

    public void HideShield()
    {
        shieldUp.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator LookAtWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lookAtTarget = true;
    }

    public override void PlayAttack(Vector3 attackDir, string AnimParametre)
    {
        base.PlayAttack(attackDir, AnimParametre);
        enemyMovementRef.animHandleRef.SetAttackDir(attackDir,(AttackType)System.Enum.Parse(typeof(AttackType),AnimParametre));
    }

    public override void LookAtTarget(Vector3 attackDir)
    {
        base.LookAtTarget(attackDir);
        enemyMovementRef.AIRotateToDir(attackDir);
    }

    public override void LookAtPlayer()
    {
        var attackDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
        LookAtTarget(attackDir);
        enemyMovementRef.animHandleRef.SetAttackDir(attackDir);
    }

    public override void PushSelfAttack()
    {
        var AttacDir = target.position - attackRangePivot.transform.position;
        var NormDir = AttacDir.normalized;
        base.enemyMovementRef.PushSelfAttack(NormDir, Mathf.Clamp(AttacDir.magnitude * ThraexData.AttackDashRatio, ThraexData.MinAttackDash, ThraexData.MaxAttackDash));
    }
}
