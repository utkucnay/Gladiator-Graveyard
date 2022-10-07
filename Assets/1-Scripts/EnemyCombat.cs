using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, IEnemyHitter
{
    [HideInInspector]
    public EnemyCombatState enemyCombatState;
    public GameObject enemyWeaponPivot;
    public GameObject attackWeaponRoot;
    public GameObject idleWeaponRoot;
    public Animator EnemyAnimator;
    public GameObject attackWeapon;
    [SerializeField]
    private float baseAttackStunTime;
    private bool attackLock;
    [HideInInspector]
    public float attackStunTime;

    protected AIAction currentActionRef;
    protected float timer = float.MaxValue;
    protected float currentTime;
    protected bool timerEnabled;
    protected EnemyHealth enemyHealthRef;

    public bool AttackLock
    {
        get { return attackLock; }
        set { attackLock = value; }
    }

    public EnemyMovement enemyMovementRef;

    void Start()
    {
        AttackLock = false;
        enemyCombatState = EnemyCombatState.Idle;
        enemyMovementRef = GetComponent<EnemyMovement>();
        
    }

    public void ShowAllWeapon()
    {
        attackWeaponRoot.transform.parent.gameObject.SetActive(true);
    }

    void Update()
    {
        if(attackStunTime>0)
        {
            attackStunTime -= Time.deltaTime;
        }
        if(enemyCombatState == EnemyCombatState.Idle)
        {

        }
        else if(enemyCombatState == EnemyCombatState.Defending)
        {

        }
        else if (enemyCombatState == EnemyCombatState.Attacking)
        {

        }
        
    }

    public void SetAttackStun(float attackStunAmount)
    {
        attackStunTime = baseAttackStunTime + attackStunAmount;
    }

    public virtual void PlayAttack(Vector3 attackDir, string AnimParametre)
    {
        SwapWeapon(false);
        LookAtTarget(attackDir);
        EnemyAnimator.SetTrigger(AnimParametre);
        
    }

    public virtual void LookAtTarget(Vector3 attackDir)
    {
        LookAtDir(enemyWeaponPivot, attackDir);
        Direction rotDir = GetAttackDir(attackDir);
        RotateWeaponToAttackDir(rotDir);
        
    }

    public virtual void LookAtPlayer()
    {

    }

    private Direction GetAttackDir(Vector3 dir)
    {
        var angle = Vector3.SignedAngle(dir.normalized, Vector3.up, Vector3.forward);
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        angle = angle % 360;

        Direction dirToRotate = Direction.North;

        if (angle < 22.5f || angle > 337.5f)
        {
            dirToRotate = Direction.North;
        }
        else if (angle > 22.5f && angle < 90f)
        {
            dirToRotate = Direction.NorthEast;
        }
        else if (angle >= 90 && angle < 157.5f)
        {
            dirToRotate = Direction.SouthEast;
        }
        else if (angle > 157.5f && angle < 202.5f)
        {
            dirToRotate = Direction.South;
        }
        else if (angle > 202.5f && angle <= 270f)
        {
            dirToRotate = Direction.SouthWest;
        }
        else if (angle > 270 && angle < 337.5f)
        {
            dirToRotate = Direction.NorthWest;
        }
        return dirToRotate;
    }

    public void SwapWeapon(bool isIdle)
    {
        if (isIdle)
        {
            attackWeaponRoot.SetActive(false);
            idleWeaponRoot.SetActive(true);
        }
        else
        {
            attackWeaponRoot.SetActive(true);
            idleWeaponRoot.SetActive(false);
        }
    }

    public void HideAllWeapon()
    {
        attackWeaponRoot.transform.parent.gameObject.SetActive(false);
    }

    public virtual void TriggerActionComplete()
    {
        if(currentActionRef != null)
        {
            currentActionRef.TriggerOncomplete(ActionResult.Success);
            currentActionRef = null;
            AttackLock = false;
        }
    }

    public virtual void TriggerActionFailure(AIAction action)
    {
        if (currentActionRef != null)
        {
            currentActionRef = null;
            AttackLock = false;
        }
    }

    public virtual void RotateWeaponToAttackDir(Direction directionToSetOrder)
    {
        if (directionToSetOrder == Direction.South || directionToSetOrder == Direction.SouthEast || directionToSetOrder == Direction.SouthWest)
        {
            attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            attackWeapon.GetComponent<SpriteRenderer>().sortingOrder = -3;
        }
    }
    private void LookAtDir(GameObject objectToChangeRot, Vector3 dirToLook)
    {
        var angle = Mathf.Atan2(dirToLook.y, dirToLook.x) * Mathf.Rad2Deg;
        Quaternion rotToTurn = Quaternion.AngleAxis(angle, Vector3.forward);
        objectToChangeRot.transform.rotation = rotToTurn;

        objectToChangeRot.transform.Rotate(Vector3.right * 180, Space.Self);
    }

    public virtual void HandleHitOnEnemy(GameObject hitObject, Collider2D collider)
    {
        
    }

    public void Parry()
    {
        StartCoroutine(ParryCor());
    }

    IEnumerator ParryCor()
    {
        yield return new WaitForSeconds(0.1f);
        AudioController.Instance.PlayAudio(AudioType.Parry, 1);
        var charPoise = GetComponent<CharacterPoise>();
        charPoise.ReducePoise(charPoise.MaxPoise);
        PlayerCharacterCombat.Instance.parryEvent.Invoke();
    }

    public virtual void PushSelfAttack()
    {
    }
}
