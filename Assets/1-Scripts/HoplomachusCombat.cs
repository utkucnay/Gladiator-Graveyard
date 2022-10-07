using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoplomachusWeaponAttackType
{
    Spear,
    SmallSword,
    throwSpear
}

public class HoplomachusCombat : EnemyCombat
{
    [Header("Hoplomachus Speacial")]

    public HoplomachusScriptableObject HoploData;

    public override void RotateWeaponToAttackDir(Direction directionToSetOrder)
    {

        switch (hoplomachusWeapon)
        {
            case HoplomachusWeaponAttackType.Spear:
                if (directionToSetOrder == Direction.South || directionToSetOrder == Direction.SouthEast || directionToSetOrder == Direction.NorthEast)
                {
                    attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                else
                {
                    attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = -3;
                }
                break;
            case HoplomachusWeaponAttackType.SmallSword:
                base.RotateWeaponToAttackDir(directionToSetOrder);
                break;
            case HoplomachusWeaponAttackType.throwSpear:
                if (directionToSetOrder == Direction.South || directionToSetOrder == Direction.SouthEast || directionToSetOrder == Direction.NorthEast)
                {
                    attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                else
                {
                    attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = -3;
                }
                break;
            default:
                break;
        }
    }

    [SerializeField] private Transform attackRangePivot;
    [SerializeField] private GameObject shieldUp;
    [SerializeField] private GameObject shieldDown;

    [HideInInspector] public bool defending;
    [HideInInspector] public bool attacking;

    private bool lookAtTarget;
    private Transform target;

    public GameObject WeaponsSpear;
    public GameObject WeaponsSmallSword;

    [HideInInspector]public bool amIDrawSpear;

    HoplomachusWeaponAttackType hoplomachusWeapon = HoplomachusWeaponAttackType.Spear;

    private Queue<Vector3> playerPositions;

    private void Awake()
    {
        enemyMovementRef = GetComponent<EnemyMovement>();
        enemyMovementRef.Speed = HoploData.Speed;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        SwapWeapon(true);
        enemyHealthRef = GetComponent<EnemyHealth>();
        enemyMovementRef = GetComponent<EnemyMovement>();
        playerPositions = new Queue<Vector3>();

        enemyMovementRef.animHandleRef.SetAttackSpeed("SpearAttackSpeed", HoploData.SpearAttackSpeed,EnemyAnimator);
        enemyMovementRef.animHandleRef.SetAttackSpeed("SmallSwordAttackSpeed", HoploData.SmallSwordAttackSpeed,EnemyAnimator);
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

        if (lookAtTarget && !enemyHealthRef.IsDead)
        {
            if(playerPositions.Count > 0)
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

    public void ShowAllShield()
    {
        shieldDown.transform.parent.gameObject.SetActive(true);
    }

    public void EndDefendAction()
    {
        currentTime = 0;
        timerEnabled = false;
        defending = false;
        SwitchDefendMode(false);
        playerPositions.Clear();
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
        enemyMovementRef.animHandleRef.SetAnimSpeedIdle(HoploData.ShieldOpenIdleAnimSpeed);
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
            float time = 0;
            switch (hoplomachusWeapon)
            {
                case HoplomachusWeaponAttackType.Spear:
                    time = Random.Range(HoploData.SpearMinAttackDelay,HoploData.SpearMaxAttackDelay);
                    break;
                case HoplomachusWeaponAttackType.SmallSword:
                    time = Random.Range(HoploData.SmallSwordMinAttackDelay,HoploData.SmallSwordMaxAttackDelay);
                    break;
                default:
                    break;
            }
            currentActionRef = actionRef;
            StartCoroutine(AttackBeginDelay(actionRef, time));
        }
        else
        {
            actionRef.TriggerOncomplete(ActionResult.Failure);
        }
    }

    IEnumerator AttackBeginDelay(AIAction actionRef, float time)
    {
        yield return new WaitForSeconds(time);
        attacking = true;
        var AttacDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
        switch (hoplomachusWeapon)
        {
            case HoplomachusWeaponAttackType.Spear:
                PlayAttack(AttacDir, "Spear");
                break;
            case HoplomachusWeaponAttackType.SmallSword:
                PlayAttack(AttacDir, "SmallSword");
                break;
        }
        Debug.Log("Attack");
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

    public override void HandleHitOnEnemy(GameObject hitObject, Collider2D collider) {

        if (PlayerCharacterCombat.Instance.parry)
        {
            base.Parry();
        }
        else
        {
            float damage = 0;
            Vector3 dir = (hitObject.transform.position - attackRangePivot.transform.position).normalized;
            float pushAmount = 0;
            float reduceStamina = 0;
            HitType hitType = HitType.MeleeDefault;
            switch (hoplomachusWeapon)
            {
                case HoplomachusWeaponAttackType.Spear:
                    pushAmount = HoploData.SpearPushAmount;
                    damage = HoploData.SpearDamage;
                    reduceStamina = HoploData.SpearReduceStamina;
                    break;
                case HoplomachusWeaponAttackType.SmallSword:
                    damage = HoploData.SmallSwordDamage;
                    pushAmount = HoploData.SmallSwordPushAmount;
                    reduceStamina = HoploData.SmallSwordReduceStamina;
                    break;
                default:
                    break;
            }
            HitInfo hitInfo = new HitInfo(damage, pushAmount, reduceStamina, dir, attackRangePivot.transform.position, PlayerCombatState.LeftAttack, hitType);
            hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
        }
    }

    public void HandleHitOnEnemyThrowSpear(GameObject hitObject, Collider2D collider)
    {
        float damage;
        Vector3 dir = (hitObject.transform.position - attackRangePivot.transform.position).normalized;
        float pushAmount;
        float reduceStamina;

        damage = HoploData.ThrowSpearDamage;
        pushAmount = HoploData.ThrowSpearPushAmount;
        reduceStamina = HoploData.ThrowSpearReduceStamina;

        HitInfo hitInfo = new HitInfo(damage, pushAmount, reduceStamina, dir, attackRangePivot.transform.position, PlayerCombatState.LeftAttack, HitType.SpearRanged);
        hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
    }

    public override void PushSelfAttack()
    {
        var AttacDir = target.position - attackRangePivot.transform.position;
        var NormDir = AttacDir.normalized;
        switch (hoplomachusWeapon)
        {
            case HoplomachusWeaponAttackType.Spear:
                base.enemyMovementRef.PushSelfAttack(NormDir, HoploData.SpearAttackDashTimeMulti, Mathf.Clamp(AttacDir.magnitude * HoploData.SpearAttackDashRatio, HoploData.SpearMinAttackDash, HoploData.SpearMaxAttackDash));
                break;
            case HoplomachusWeaponAttackType.SmallSword:
                base.enemyMovementRef.PushSelfAttack(NormDir, Mathf.Clamp(AttacDir.magnitude * HoploData.SmallSwordAttackDashRatio, HoploData.SmallSwordMinAttackDash, HoploData.SmallSwordMaxAttackDash));
                break;
            default:
                break;
        }
    }

    public void SwitchDefendMode(bool defending)
    {
        if (defending)
        {
            shieldUp.SetActive(true);
            shieldDown.SetActive(false);
            defending = true;
            StartCoroutine(LookAtWithDelay(HoploData.LookDelay));
        }
        else
        {
            shieldUp.SetActive(false);
            shieldDown.SetActive(true);
            defending = false;
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
        switch (hoplomachusWeapon)
        {
            case HoplomachusWeaponAttackType.Spear:
                enemyMovementRef.animHandleRef.SetAttackDir(attackDir, hoplomachusWeapon);
                break;
            case HoplomachusWeaponAttackType.SmallSword:
                enemyMovementRef.animHandleRef.SetAttackDir(attackDir, hoplomachusWeapon);
                break;
            case HoplomachusWeaponAttackType.throwSpear:
                enemyMovementRef.animHandleRef.SetDraw();
                break;
            default:
                break;
        }
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

    public void GetSmallSword()
    {
        WeaponsSpear.SetActive(false);
        WeaponsSmallSword.SetActive(true);
        hoplomachusWeapon = HoplomachusWeaponAttackType.SmallSword;
        idleWeaponRoot = WeaponsSmallSword;
    }

    public void ThrowSpear(AIAction aiAction)
    {
        AttackLock = true;
        currentActionRef = aiAction;
        float time = UnityEngine.Random.Range(HoploData.ThrowSpearMinAttackDelay, HoploData.ThrowSpearMaxAttackDelay);
        StartCoroutine(ThrowSpearCor(time));
    }

    IEnumerator ThrowSpearCor(float time)
    {
        var AttacDir = Vector3.Normalize(target.position - enemyWeaponPivot.transform.position);
        amIDrawSpear = true;
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

        ReleaseSpear(AttacDir);
        currentActionRef.TriggerOncomplete(ActionResult.Success);
        amIDrawSpear = false;

    }

    void ReleaseSpear(Vector2 attackDir)
    {
        float angle = Vector2.SignedAngle(new Vector2(1, 0), attackDir);
        var ArrowObject = Instantiate(HoploData.Spear, EnemyAnimator.gameObject.transform.position, Quaternion.identity,WaveController.Instance.trash.transform);
        ArrowObject.transform.eulerAngles += new Vector3(0, 0, angle);
        ArrowObject.GetComponent<Projectile>().SetDirection(attackDir);
        ArrowObject.GetComponent<Rigidbody2D>().AddForce(attackDir * 25 * HoploData.ThrowSpearSpeedMulti);
        ArrowObject.GetComponent<AttackTriggerNotifier>().AddEvent(HandleHitOnEnemyThrowSpear);
        EnemyAnimator.SetTrigger("Release");
        enemyMovementRef.animHandleRef.SetRelease();
        enemyMovementRef.ChangePrevDir(enemyMovementRef.GetTarget().position - transform.position);
        GetSmallSword();
    }
}
