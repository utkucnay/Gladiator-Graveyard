using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerCharacterCombat : Singleton<PlayerCharacterCombat>, IEnemyHitter
{
    [SerializeField] private float comboEndDelay = 0.3f;
    public GameObject murmilloGladiusPivot;
    public CharacterRotator2D characterRotatorRef;
    public MurmilloAttributes murmilloAttributes;

    [HideInInspector]
    public PlayerCombatState combatState;

    private EquipmentKits equippedKit;
    private Vector3 clickedPosition;
    private Rigidbody2D rb;

    private bool attackLock;

    public UnityEvent parryEvent;
    public bool parry;
    public float DamageMultiplier { get; set; }

    public BasePlayerAttributes playerAttributes;
    public Animator ParryAnim;
    public override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody2D>();
        parryEvent = new UnityEvent();
        parryEvent.AddListener(ParrySuccesful);
        DamageMultiplier = 1;

        WaveController.Instance.ArenaMatchStarted.AddListener(ArenaMatchStarted);
        WaveController.Instance.ArenaMatchEnded.AddListener(ArenaMatchEnded);
        LockAttack(true);
    }

    private void ArenaMatchEnded()
    {
        LockAttack(true);

    }

    private void ArenaMatchStarted()
    {
        LockAttack(false);
    }

    private void ParrySuccesful()
    {
        FeelFeedbackController.Instance.PlayFeedback(FeelType.ParryFeedback);
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public override void PlayerDied()
    {
        base.PlayerDied();
        LockAttack(true);
    }

    public override void PlayerBorn()
    {
        base.PlayerBorn();
        LockAttack(false);
    }

    public override void RunStarted()
    {
        base.RunStarted();
        equippedKit = GameController.Instance.equippedKit;
        combatState = PlayerCombatState.Idle;
    }

    void Update()
    {
        if(!attackLock)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                clickedPosition = Input.mousePosition;
                PerformLightAttack(clickedPosition);
                Parry();
            }
            if(Input.GetButtonDown("Fire2"))
            {
                PerformSpecial();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                StopSpecial();
            }

            if(combatState == PlayerCombatState.CoverShield)
            {
                RotateCharacterToDir(FindAttackDirection(Input.mousePosition));
            }
        }
    }

    private void PerformSpecial()
    {
        switch (equippedKit)
        {
            case EquipmentKits.Murmillo:
                {
                    MurmilloAnimationController.Instance.CoverShield();
                    combatState = PlayerCombatState.CoverShield;
                    PlayerStamina.Instance.LockRegen();
                }
                break;
        }
    }

    public void StopSpecial()
    {
        switch (equippedKit)
        {
            case EquipmentKits.Murmillo:
                {
                    MurmilloAnimationController.Instance.OpenShield();
                    combatState = PlayerCombatState.Idle;
                    PlayerStamina.Instance.UnlockRegen();
                }
                break;
        }
    }

    private void PerformLightAttack(Vector3 clickedPos)
    {
        Vector3 attackDir = FindAttackDirection(clickedPos);

        switch (equippedKit)
        {
            case EquipmentKits.Murmillo:
                {
                    if(combatState != PlayerCombatState.CoverShield && PlayerStamina.Instance.ConsumeStamina(murmilloAttributes.lightAttackStamina))
                    {
                        LookAtDir(murmilloGladiusPivot, attackDir);
                        Direction rotDir = RotateCharacterToDir(attackDir);
                        MurmilloAnimationController.Instance.PlayLightAttack(rotDir);
                        combatState = PlayerCombatState.LeftAttack;
                        MoveTowardsAttack(attackDir, 0.7f);
                    }
                }
                break;
        }
    }

    private void MoveTowardsAttack(Vector3 dir, float amount)
    {
        Vector3 normDir = dir.normalized;
        rb.DOMove(gameObject.transform.position + normDir * amount, 0.12f).SetEase(Ease.InCubic).SetDelay(0);
    }

    public void HandleHitOnEnemy(GameObject hitObject, Collider2D collider)
    {
        float damage = 0;
        switch (equippedKit)
        {
            case EquipmentKits.Murmillo:
                {
                    switch (combatState)
                    {
                            case PlayerCombatState.LeftAttack:
                            {
                               // Debug.Log("damaged " + hitObject.gameObject.name + " for " + murmilloAttributes.gladiusDamage);
                                damage = murmilloAttributes.gladiusDamage * DamageMultiplier;
                            }
                            break;
                        case PlayerCombatState.rightAttack:
                            {
                                // Debug.Log("damaged " + hitObject.gameObject.name + " for " + murmilloAttributes.gladiusDamage);
                                damage = murmilloAttributes.gladiusDamage * DamageMultiplier;

                            }
                            break;
                        case PlayerCombatState.thrustAttack:
                            {
                                // Debug.Log("damaged " + hitObject.gameObject.name + " for " + murmilloAttributes.gladiusDamage);
                                damage = murmilloAttributes.gladiusDamage * DamageMultiplier;
                            }
                            break;
                    }
                }
                break;
        }

        Vector3 dir = (hitObject.transform.position - gameObject.transform.position).normalized;
        HitInfo hitInfo = new HitInfo(damage, 2,20,dir,gameObject.transform.position, combatState, HitType.MeleeDefault);
        hitObject.GetComponent<CharacterHealth>().ReceiveDamage(hitInfo);
    }

    public Vector3 FindAttackDirection(Vector3 clickedPos)
    {
        return clickedPos - Camera.main.WorldToScreenPoint(murmilloGladiusPivot.transform.position);
    }

    private void LookAtDir(GameObject objectToChangeRot , Vector3 dirToLook)
    {
        var angle = Mathf.Atan2(dirToLook.y, dirToLook.x) * Mathf.Rad2Deg;
        Quaternion rotToTurn = Quaternion.AngleAxis(angle, Vector3.forward);
        objectToChangeRot.transform.rotation = rotToTurn;

        objectToChangeRot.transform.Rotate(Vector3.right * 180, Space.Self);
    }

    private Direction RotateCharacterToDir(Vector3 dir)
    {
        //find angle of attack
        var angle = Vector3.SignedAngle(dir.normalized, Vector3.up, Vector3.forward);
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        angle = angle % 360;

        Direction dirToRotate = Direction.North;

        if(angle < 22.5f || angle > 337.5f)
        {
            dirToRotate = Direction.North;
        }
        else if(angle > 22.5f && angle < 90f)
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

        characterRotatorRef.RotateCharacter(dirToRotate);

        return dirToRotate;
    }

    public void LockAttack(bool locked)
    {
        attackLock = locked;
    }

    public void Parry()
    {
        if (combatState == PlayerCombatState.CoverShield)
        {
            attackLock = true;
            combatState = PlayerCombatState.Parry;

            AudioController.Instance.PlayAudio(AudioType.MurmilloGladiusSwing, 1);


            ParryAnim.gameObject.SetActive(true);
            MurmilloAnimationController.Instance.HideShield();
            ParryAnim.SetFloat("DirX", PlayerCharacterMovement.Instance.GetCurrDir().x);
            ParryAnim.SetFloat("DirY", PlayerCharacterMovement.Instance.GetCurrDir().y);
            PlayerAnimationController.Instance.SetParryDir(PlayerCharacterMovement.Instance.GetCurrDir());
            PlayerCharacterMovement.Instance.ParryLockMovement();
        }
    }

    public void StartParryEvent()
    {
        parry = true;
    }

    public void EndParryEvent()
    {
        parry = false;
        attackLock = false;
        ParryAnim.gameObject.SetActive(false);
        MurmilloAnimationController.Instance.OpenShield();
        combatState = PlayerCombatState.Idle;
        PlayerCharacterMovement.Instance.UnlockMovement();
        PlayerStamina.Instance.UnlockRegen();
    }

    public void AttackDelay() 
    {
        StartCoroutine(AttackDelayCor());
    }

    IEnumerator AttackDelayCor()
    {
        attackLock = true;
        yield return new WaitForSeconds(comboEndDelay / SetAttackSpeedPlayer.Instance.PlayerAttackSpeed);
        attackLock = false;
    }

    public void SetAttackDamageOnData()
    {
        murmilloAttributes.gladiusDamage = playerAttributes.str;
    }
}
