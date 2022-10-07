using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : Singleton<PlayerCharacterMovement>
{

    [Header("Character Rotator")]
    [SerializeField]
    private CharacterRotator2D rotatorRef;

    [Header("Main Movement")]
    [SerializeField]
    public float speed;
    public BasePlayerAttributes playerAttributes;

    //Main Movement Parameters
    private float inputX;
    private float inputY;
    private Vector2 movementDir;

    private Rigidbody2D rb;
    private bool movementLocked;

    [Header("Dash")]
    //Dash Parameters
    [SerializeField]
    private float dashCooldown;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashTime;
    private float dashTimer;
    private Vector2 dashDir;
    private bool dashing;

    private float movementLockTimer;

    public Vector3 GetCurrDir()
    {
        return rotatorRef.currentDirectionVector;
    }

    public override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public override void RunStarted()
    {
        base.RunStarted();
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        movementDir = new Vector2(inputX, inputY).normalized;

        if (Input.GetButtonDown("Dash"))
        {
            DashCharacter();
        }
        dashTimer += Time.deltaTime;
        if(movementLockTimer > 0)
        {
            movementLockTimer -= Time.deltaTime;
            movementLocked = true;
        }
        else
        {
            movementLocked = false;
        }
    }

    private void RotateCharacter()
    {
        Direction characterDir = Direction.South;
        switch (movementDir.x)
        {
            case 0:
                {
                    switch(movementDir.y)
                    {
                        case 0:
                            {
                                characterDir = rotatorRef.currentDirection;
                                break;
                            }
                        case 1:
                            {
                                characterDir = Direction.North;
                                break;
                            }
                        case -1:
                            {
                                characterDir = Direction.South;
                                break;
                            }
                    }
                    break;
                }
            case 1:
                {
                    characterDir = Direction.SouthEast;
                    break;
                }
            case -1:
                {
                    characterDir = Direction.SouthWest;
                    break;
                }
            case float n when (n > 0f && n < 0.95f):
                {
                    switch (movementDir.y)
                    {
                        case 0:
                            {
                                characterDir = Direction.SouthEast;
                                break;
                            }
                        case float d when (d > 0f && d < 0.95f):
                            {
                                characterDir = Direction.NorthEast;
                                break;
                            }
                        case float d when (d < 0f && d > -0.95f):
                            {
                                characterDir = Direction.SouthEast;
                                break;
                            }
                    }
                    break;
                }
            case float n when (n < 0f && n > -0.95f):
                {
                    switch (movementDir.y)
                    {
                        case 0:
                            {
                                characterDir = Direction.SouthWest;
                                break;
                            }
                        case float d when (d > 0f && d < 0.95f):
                            {
                                characterDir = Direction.NorthWest;
                                break;
                            }
                        case float d when (d < 0f && d > -0.95f):
                            {
                                characterDir = Direction.SouthWest;
                                break;
                            }
                    }
                    break;
                }
           
        }
        rotatorRef.RotateCharacter(characterDir);
    }

    

    private void DashCharacter()
    {
        if (PlayerStamina.Instance.ConsumeStamina(30))
        {
            if (dashTimer > dashCooldown)
            {
                FeelFeedbackController.Instance.PlayFeedback(FeelType.DashFeedback);
                dashTimer = 0;
                dashDir = movementDir;
                dashing = true;
                StartCoroutine(StopDash());
            }
        }
    }
    private void FixedUpdate()
    {
        if(movementLocked == false)
        {
            if(dashing)
            {
                rb.velocity = dashDir * dashSpeed;
            }
            else
            {
                if(PlayerCharacterCombat.Instance.combatState == PlayerCombatState.CoverShield)
                {
                    rb.velocity = movementDir * speed * playerAttributes.spd * PlayerCharacterCombat.Instance.murmilloAttributes.murmilloCoverWalkSpeedRatio/100;
                }
                else
                {
                    rb.velocity = movementDir * speed * playerAttributes.spd;
                }
                RotateCharacter();
            }
        }
        PlayerAnimationController.Instance.SetCharAnimSpeed(rb.velocity.magnitude);
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        yield break;
    }

    public void LockMovement()
    {
        movementLockTimer = float.MaxValue;
        dashing = false;
        rb.velocity = Vector3.zero;
    }

    public void ParryLockMovement()
    {
        LockMovement();
        movementDir = Vector2.zero;
    }

    public void LockMovement(float timeToUnlock)
    {
        movementLockTimer += timeToUnlock;
        dashing = false;
        rb.velocity = Vector3.zero;
    }

    public void UnlockMovement()
    {
        movementLockTimer = 0;
    }
    public void PushSelf(HitInfo hitInfo)
    {
        LockMovement(0.15f);
        rb.DOMove(gameObject.transform.position + new Vector3(hitInfo.hitDirection.x, hitInfo.hitDirection.y) * hitInfo.pushAmount, 0.15f);
    }

    public void PushSelf(HitInfo hitInfo, float pushMultiplier)
    {
        LockMovement(0.15f);
        rb.DOMove(gameObject.transform.position + new Vector3(hitInfo.hitDirection.x, hitInfo.hitDirection.y) * hitInfo.pushAmount * pushMultiplier, 0.15f);
    }

    public override void PlayerDied()
    {
        base.PlayerDied();
        LockMovement();
    }

    public override void PlayerBorn()
    {
        base.PlayerBorn();
        UnlockMovement();
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(-4.05f, -0.2f, 0);
    }
}
