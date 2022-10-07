using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThraexAI : EnemyAI
{
    // ram siserse ai actiona bak

    private bool playerClose;
    private ThraexHealth thraexHealthRef;
    private ThraexCombat thraexCombatRef;

    private CharacterPoise characterPoiseRef;

    ThraexScriptableObject ThraexData;

    bool RDDelay = true;
    int RDDelayint = 0;
    void Start()
    {
        thraexCombatRef = gameObject.GetComponent<ThraexCombat>();
        enemyMovementRef = gameObject.GetComponent<EnemyMovement>();
        thraexHealthRef = gameObject.GetComponent<ThraexHealth>();
        characterPoiseRef = gameObject.GetComponent<CharacterPoise>();
        //StartCoroutine(CalculateDecision(0));
        characterPoiseRef.AddBreakPoiseEvent(PoiseBreak);
        characterPoiseRef.AddBreakPoiseEvent(() => enemyMovementRef.animHandleRef.SetStun(true));
        characterPoiseRef.AddBreakPoiseEvent(() => thraexCombatRef.SwapWeapon(true));
        characterPoiseRef.AddBreakPoiseEvent(() =>
        {
            thraexHealthRef.defaultColor = new Color(1.5f, 1.5f, 1.5f, 1);
            ParticleEffectController.Instance.CharAnimReciveDamage(null, enemyMovementRef.animHandleRef.GetMaterial(),
            characterPoiseRef.PoiseRecoverTime,  new Color(1.5f, 1.5f, 1.5f, 1), thraexHealthRef);
        });
        ThraexData = thraexCombatRef.ThraexData;
        thraexHealthRef.RDDelay.AddListener(ReciveDamageDelay);
    }

    void Update()
    {
    }


    public void PoiseBreak()
    {
        currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, thraexCombatRef.TriggerActionFailure, this);
        AIAction aiAction = new AIAction();
        aiAction.onComplete.AddListener(ActionCompleted);
        aiAction.TakeAction(thraexHealthRef.charPoiseRef.PoiseLock, thraexHealthRef.charPoiseRef.PoiseRecoverTime);
        currentAction = aiAction;
        Debug.Log("Poise Break");
    }

    public void ReciveDamageDelay()
    {
        if (RDDelay && !characterPoiseRef.IsPoise)
        {
            currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, thraexCombatRef.TriggerActionFailure, this);
            AIAction aiAction = new AIAction();
            aiAction.onComplete.AddListener(ActionCompleted);
            aiAction.TakeAction(enemyMovementRef.DontMoveWithTimer,0.1f);
            currentAction = aiAction;
            RDDelay = false;
            Debug.Log("Recive Damage Sec " + 0.1f);
        }
    }

    

    public void PlayerEntered(GameObject hitObject, Collider2D collider)
    {
        
        playerClose = true;
        if (!thraexHealthRef.charPoiseRef.IsPoise && !thraexCombatRef.AttackLock && currentAction != null && RDDelay)
        {
            currentAction.StopAction(enemyMovementRef.TriggerActionFailed, thraexCombatRef.TriggerActionFailure, this);
        }
    }
    public void PlayerExited(GameObject hitObject, Collider2D collider)
    {
        playerClose = false;
        if (!thraexHealthRef.charPoiseRef.IsPoise && !thraexCombatRef.AttackLock && currentAction != null && RDDelay)
        {
            currentAction.StopAction(enemyMovementRef.TriggerActionFailed, thraexCombatRef.TriggerActionFailure, this);
        }
    }

    IEnumerator<WaitForSeconds> CalculateDecision(float delay)
    {
        yield return new WaitForSeconds(delay);
        int rand = UnityEngine.Random.Range(1, 101);
        if(playerClose)
        {
            if (rand <= ThraexData.DefendChance)
            {
                // defend %45
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                float time = UnityEngine.Random.Range(ThraexData.minDefTime, ThraexData.maxDefTime);
                aiAction.TakeAction(thraexCombatRef.DefendWithShield, time);
                currentAction = aiAction;
            }
            else if (rand <= ThraexData.AttackChance + ThraexData.DefendChance)
            {
                // attack player %45 with a random pattern
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                aiAction.TakeAction(thraexCombatRef.AttackTarget);
                currentAction = aiAction;
                Debug.Log("ins");
            }
            else if (rand <= ThraexData.AttackChance + ThraexData.DefendChance + ThraexData.WaitChance)
            {
                // shield down and wait for x-y sec %10
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                float time= UnityEngine.Random.Range(ThraexData.minWaitTime, ThraexData.maxWaitTime);
                aiAction.TakeAction(enemyMovementRef.DontMoveWithTimer,time);
                currentAction = aiAction;
                Debug.Log("Wait Sec "+ time);
            }
        }
        else
        { 
            if(rand <= ThraexData.MoveTowordsPlayerChance)
            {
                // move towards player x seconds %75
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                float trackTime = UnityEngine.Random.Range(ThraexData.minMoveTowordsPlayer, ThraexData.maxMoveTowordsPlayer);
                aiAction.TakeAction(enemyMovementRef.MoveToPlayer, trackTime);
                currentAction = aiAction;

                Debug.Log("tracking player for " + trackTime);
            }
            else if(rand <= ThraexData.MoveTowordsPlayerChance + ThraexData.MoveWaypointChance)
            {
                /*AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                List<Vector3> posList = new List<Vector3>();
                int wayPointNum = UnityEngine.Random.Range(2, 4);
                for(int i =0; i<wayPointNum; i++)
                {
                    float randPosX = UnityEngine.Random.Range(1, 6);
                    float randPosY = UnityEngine.Random.Range(1, 4);
                    posList.Add(SetInBoundary(gameObject.transform.position + new Vector3(randPosX, randPosY, 0)));
                }
                aiAction.TakeAction(enemyMovementRef.MoveToPosition, posList.ToArray());
                currentAction = aiAction;

                Debug.Log("Moving Random for " + wayPointNum);*/

                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                bool ReverseLocBoolX = enemyMovementRef.GetTarget().position.x - transform.position.x > 0;
                bool ReverseLocBoolY = enemyMovementRef.GetTarget().position.y - transform.position.y > 0;
                List<Vector3> posList = new List<Vector3>();
                int wayPointNum = UnityEngine.Random.Range(ThraexData.minWaypoint, ThraexData.maxWaypoint);
                int way = UnityEngine.Random.Range(1, 3);

                for (int i = 0; i < wayPointNum; i++)
                {
                    float randPosX = UnityEngine.Random.Range(ThraexData.longMin, ThraexData.longMax) * (i + 1) * 0.75f;
                    float randPosY = UnityEngine.Random.Range(ThraexData.shortMin, ThraexData.shortMax) * (i + 1) * 1.4f;

                    if ((ReverseLocBoolX && ReverseLocBoolY) || (!ReverseLocBoolX && !ReverseLocBoolY))
                    {
                        if (way != 1)
                        {
                            float temp = randPosX;
                            randPosX = randPosY;
                            randPosY = temp;
                        }
                    }
                    else
                    {
                        if (way == 1)
                        {
                            float temp = randPosX;
                            randPosX = randPosY;
                            randPosY = temp;
                        }
                    }

                    randPosX = ReverseLocBoolX ? randPosX : randPosX*-1;
                    randPosY = ReverseLocBoolY ? randPosY : randPosY*-1;

                    Vector3 vector3 = new Vector3(gameObject.transform.position.x + randPosX, gameObject.transform.position.y + randPosY, gameObject.transform.position.z);
                    if(CheckWaypoint(vector3))
                    {
                        posList.Add(vector3);
                    }
                    else
                    {
                        Debug.Log("Waypoint not walkable area");
                    }
                }
                aiAction.TakeAction(enemyMovementRef.MoveToPosition, posList.ToArray());
                currentAction = aiAction;
                Debug.Log("Moving Random for " + posList.Count);
            }
        }
        yield break;
    }

    Vector3 SetInBoundary(ref Vector3 vector3)
    {
        float x = Mathf.Clamp(vector3.x, GameController.Instance.CurrentArena.ArenaBoundary.BotX, GameController.Instance.CurrentArena.ArenaBoundary.TopX);
        float y = Mathf.Clamp(vector3.y, GameController.Instance.CurrentArena.ArenaBoundary.BotY, GameController.Instance.CurrentArena.ArenaBoundary.TopY);
        return new Vector3(x, y, vector3.z);
    }

    protected override void ActionCompleted(ActionResult result)
    {
        Debug.Log("action completed with: " + result);
        switch (result)
        {
            case ActionResult.Success:
                float time = playerClose ? 0.25f : 0.75f; 
                StartCoroutine(CalculateDecision(time));
                
                break;
            case ActionResult.Failure:
                StartCoroutine(CalculateDecision(0.25f));
                break;
            default:
                break;
        }

        if (RDDelayint > 0)
        {
            RDDelay = true;
        }
        else
        {
            RDDelayint += 1;
        }
    }
}
