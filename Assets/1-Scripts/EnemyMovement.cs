using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshComponents;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;


// ramde meme olursa change statee bak

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public float Speed;
    private Transform Target;
    [HideInInspector]
    public bool RotateLock;
    [HideInInspector]
    public CharacterRotator2D Cr;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public AnimHandle animHandleRef;
    
    private Coroutine OldPushSelfCorRef;
    IStage CurrentStage;

    private float timer = float.MaxValue;
    private float currentTime;
    private bool timerEnabled;
    private AIAction currentActionRef;
    private Vector3 prevDir;

    public UnityEvent WeaponDraw;
    private void Awake()
    {
        
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        animHandleRef = gameObject.GetComponent<AnimHandle>();
        CurrentStage = null;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Cr = GetComponent<CharacterRotator2D>();
        agent.speed = Speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        RotateLock = false;
    }
    private void Update()
    {
        
        if (timerEnabled)
            currentTime += Time.deltaTime;

        if (currentTime > timer && timerEnabled)
        {
            agent.ResetPath();
            currentTime = 0;
            timerEnabled = false;
            if (currentActionRef != null)
            {
                CurrentStage = null;
                currentActionRef.TriggerOncomplete(ActionResult.Success);
            }

        }
        if (agent.enabled == true)
        {
            animHandleRef.SetCharAnimSpeed(agent.velocity.magnitude);
        }
        

        if (CurrentStage != null)
        {
            if (CurrentStage.GetLocation() != null)
            {
                agent.SetDestination(CurrentStage.GetLocation().Value);
            }
            else
            {
                if ((CurrentStage as IWaypointSystem) == null)
                {
                    if (agent.enabled == true)
                    {
                        agent.ResetPath();
                    }
                    
                    CurrentStage = null;
                }
            }
        }


        if ((CurrentStage as IWaypointSystem) == null) return;
        if (!(CurrentStage as IWaypointSystem).CheckWaypoint(transform.position))
        {
            agent.ResetPath();
            CurrentStage = null;
            currentTime = 0;
            timerEnabled = false;
            currentActionRef.TriggerOncomplete(ActionResult.Success);
            AIRotateToDir((Target.transform.position - gameObject.transform.position).normalized);
        }
    }

    private void FixedUpdate()
    {
        if (!RotateLock)
            AIRotate();
    }

    public void MoveToPlayer(AIAction actionRef, float timeToMove)
    {
        currentActionRef = actionRef;
        timer = timeToMove;
        currentTime = 0;
        timerEnabled = true;
        ChangeState(new EnemyTrackPlayer(Target));
    }

    public void MoveToPosition(AIAction actionRef, Vector3[] posList)
    {
        currentActionRef = actionRef;

        ChangeState(new EnemyWaypointSystem(posList));
    }

    public void MovePositionGameBegin(AIAction actionRef, Vector3 loc)
    {
        float time = loc.magnitude / Speed;
        AIRotateToDir(Vector3.right);
        AIRotateToDir(loc.normalized);
        animHandleRef.SetCharAnimSpeed(Speed);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(transform.position + loc, time)).SetEase(Ease.Linear);
        sequence.AppendCallback(() => {
            agent.enabled = true;
            WeaponDraw?.Invoke();
            currentActionRef.TriggerOncomplete(ActionResult.Failure);
        });
        currentActionRef = actionRef;
    }

    public void DontMoveWithTimer(AIAction actionRef, float timeToMove)
    {
        currentActionRef = actionRef;
        timer = timeToMove;
        currentTime = 0;
        timerEnabled = true;
        ChangeState(new EnemyIdle());
    }

    public void DontMove(AIAction actionRef)
    {
        currentActionRef = actionRef;
        ChangeState(new EnemyIdle());
    }

    public void TriggerActionFailed(AIAction actionRef)
    {
        ChangeState(new EnemyIdle());
        currentActionRef = null;
    }

    public void VelocityMoveToDir(Vector3 Dir)
    {
        agent.velocity = Dir;
        ChangePrevDir(Dir);
    }

    public void ChangeState(IStage stage)
    {
        CurrentStage = stage;
    }
    public void EnemyRotate(bool isbool)
    {
        RotateLock = isbool;
    }
    public void SetLock(bool isLock)
    {
        if (isLock)
        {
            agent.speed = 0.001f;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.speed = Speed;
        }
    }
    public Transform GetTarget()
    {
        return Target;
    }
    public void PushSelf(HitInfo hitInfo, float PushAmontMultipler, float pushTime, Ease pushEase)
    {
        if (OldPushSelfCorRef != null)
        {
            StopCoroutine(OldPushSelfCorRef);
        }
        //OldPushSelfCorRef = StartCoroutine(PushSelfCor(gameObject.transform.position + new Vector3(hitInfo.hitDirection.x, hitInfo.hitDirection.y) * hitInfo.pushAmount * PushAmontMultipler, amount));

        

        //Debug.Log("pos: " + gameObject.transform.position + "  hitinmfo: " + hitInfo.hitDirection);
        Vector3 pushVector = gameObject.transform.position + hitInfo.hitDirection * PushAmontMultipler;
        switch (hitInfo.combatState)
        {
            case PlayerCombatState.LeftAttack:
                pushVector -= (Vector3)Vector2.Perpendicular(hitInfo.attackWeaponPosition - gameObject.transform.position) * 0.2f * PushAmontMultipler;
                break;
            case PlayerCombatState.rightAttack:
                pushVector += (Vector3)Vector2.Perpendicular(hitInfo.attackWeaponPosition - gameObject.transform.position) * 0.2f * PushAmontMultipler;
                break;
            case PlayerCombatState.thrustAttack:
                break;
           
            default:
                break;
        }

        //pushVector = pushVector * hitInfo.pushAmount * PushAmontMultipler;

        OldPushSelfCorRef = StartCoroutine(PushSelfCor(pushVector , pushTime, pushEase));
    }
    public void PushSelf(HitInfo hitInfo, float PushAmontMultipler, float pushTime, Ease pushEase, out Vector3 pushVector)
    {
        if (OldPushSelfCorRef != null)
        {
            StopCoroutine(OldPushSelfCorRef);
        }
        //OldPushSelfCorRef = StartCoroutine(PushSelfCor(gameObject.transform.position + new Vector3(hitInfo.hitDirection.x, hitInfo.hitDirection.y) * hitInfo.pushAmount * PushAmontMultipler, amount));



        //Debug.Log("pos: " + gameObject.transform.position + "  hitinmfo: " + hitInfo.hitDirection);
        pushVector = gameObject.transform.position + hitInfo.hitDirection * PushAmontMultipler;
        switch (hitInfo.combatState)
        {
            case PlayerCombatState.LeftAttack:
                pushVector -= (Vector3)Vector2.Perpendicular(hitInfo.attackWeaponPosition - gameObject.transform.position) * 0.2f * PushAmontMultipler;
                break;
            case PlayerCombatState.rightAttack:
                pushVector += (Vector3)Vector2.Perpendicular(hitInfo.attackWeaponPosition - gameObject.transform.position) * 0.2f * PushAmontMultipler;
                break;
            case PlayerCombatState.thrustAttack:
                break;

            default:
                break;
        }

        //pushVector = pushVector * hitInfo.pushAmount * PushAmontMultipler;

        OldPushSelfCorRef = StartCoroutine(PushSelfCor(pushVector, pushTime, pushEase));
    }
    public void ReciveDamageAnim(HitInfo hitInfo)
    {
        switch (hitInfo.combatState)
        {
            case PlayerCombatState.LeftAttack:
                if (Cr.currentDirectionVector.y < 0)
                {
                    if (hitInfo.hitDirection.x >= -0.15f)
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(-1, -1));
                    }
                    else
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(1, -1));
                    }
                }
                else
                {
                    if (hitInfo.hitDirection.x >= -0.15f)
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(-1, 1));
                    }
                    else
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(1, 1));
                    }
                }
                break;
            case PlayerCombatState.rightAttack:
                if (Cr.currentDirectionVector.y < 0)
                {
                    if (hitInfo.hitDirection.x >= -0.15f)
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(1, -1));
                    }
                    else
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(-1, -1));
                    }
                }
                else
                {
                    if (hitInfo.hitDirection.x >= -0.15f)
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(-1, 1));
                    }
                    else
                    {
                        animHandleRef.ReciveDamageAnim(new Vector2(1, 1));
                    }
                }
                break;
            case PlayerCombatState.thrustAttack:
                if (Cr.currentDirectionVector.y < 0)
                {
                    animHandleRef.ReciveDamageAnim(new Vector2(0, -1));
                }
                else
                {
                    animHandleRef.ReciveDamageAnim(new Vector2(0, 1));
                }
                break;

            default:
                break;
        }
    }

    public void PushSelfAttack(Vector3 normDir, float amount)
    {
        if (OldPushSelfCorRef != null)
        {
            StopCoroutine(OldPushSelfCorRef);
        }
        Debug.Log(" norm dir : " + normDir);
        OldPushSelfCorRef = StartCoroutine(PushSelfCor(gameObject.transform.position + normDir * amount, 0.15f, Ease.Linear));
    }
    public void PushSelfAttack(Vector3 normDir,float timeMulti, float amount)
    {
        if (OldPushSelfCorRef != null)
        {
            StopCoroutine(OldPushSelfCorRef);
        }
        Debug.Log(" norm dir : " + normDir);
        OldPushSelfCorRef = StartCoroutine(PushSelfCor(gameObject.transform.position + normDir * amount, animHandleRef.charAnimatorRef.GetCurrentAnimatorStateInfo(0).length * timeMulti, Ease.Linear));
    }
    private void AIRotate()
    {
        Vector3 Dir = prevDir;
        if (CurrentStage != null && CurrentStage.GetLocation() != null)
        {
            Dir = ((Vector3)CurrentStage.GetLocation() - transform.position).normalized;
            prevDir = Dir;
        }
        //Vector3 Dir = (Target.position - transform.position).normalized;

        if (Dir.y > 0f)
        {
            if (Dir.x < 0.15f && Dir.x > -0.15f)
            {
                Cr.RotateCharacter(Direction.North);
            }
            else if (Dir.x > 0.15f)
            {
                Cr.RotateCharacter(Direction.NorthEast);
            }
            else if (Dir.x < -0.15f)
            {
                Cr.RotateCharacter(Direction.NorthWest);
            }
        }
        else if (Dir.y < 0f)
        {
            if (Dir.x < 0.15f && Dir.x > -0.15f)
            {
                Cr.RotateCharacter(Direction.South);
            }
            else if (Dir.x > 0.15f)
            {
                Cr.RotateCharacter(Direction.SouthEast);
            }
            else if (Dir.x < -0.15f)
            {
                Cr.RotateCharacter(Direction.SouthWest);
            }
        }
    }

    public void AIRotateToDir(Vector3 Dir)
    {
        prevDir = Dir;
        if (Dir.y > 0f)
        {
            if (Dir.x < 0.15f && Dir.x > -0.15f)
            {
                Cr.RotateCharacter(Direction.North);
            }
            else if (Dir.x > 0.15f)
            {
                Cr.RotateCharacter(Direction.NorthEast);
            }
            else if (Dir.x < -0.15f)
            {
                Cr.RotateCharacter(Direction.NorthWest);
            }
        }
        else if (Dir.y <= 0f)
        {
            if (Dir.x < 0.15f && Dir.x > -0.15f)
            {
                Cr.RotateCharacter(Direction.South);
            }
            else if (Dir.x > 0.15f)
            {
                Cr.RotateCharacter(Direction.SouthEast);
            }
            else if (Dir.x < -0.15f)
            {
                Cr.RotateCharacter(Direction.SouthWest);
            }
        }
    }

    private IEnumerator PushSelfCor(Vector3 Loc, float TimeEnd, Ease pushEase)
    {
        /*float time = 0;
        float ScaleTime = 1 / TimeEnd;
        Vector2 BeginPoint = transform.position;
        Vector2 EndPoint = Loc;
        Vector2 ScaleBeginToEndPoint = EndPoint - BeginPoint;
        while (time < TimeEnd)
        {
            var iter = 1 - Mathf.Pow(1 - time * ScaleTime, 2);
            agent.velocity = ScaleBeginToEndPoint * iter * 7.5f;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }*/

        Debug.Log(Loc);
        Vector3 pos = agent.nextPosition;
        DOTween.To(() => pos,
       x => agent.nextPosition = x, Loc, TimeEnd).SetEase(pushEase);


        /*TrackPlayer = true;
        agent.velocity = Vector3.zero;
        OldPushSelfCorRef = null;*/

        yield break;
    }

    public void ChangePrevDir(Vector3 Dir)
    {
        prevDir = Dir;
    }
    public float GetSpeed()
    {
        return Speed;
    }
}
