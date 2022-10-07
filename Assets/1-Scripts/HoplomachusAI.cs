using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoplomachusAI : EnemyAI
{
    private bool playerClose;
    private HoplomachusHealth hoplomachusHealthRef;
    private HoplomachusCombat hoplomachusCombatRef;
    private CharacterPoise characterPoiseRef;
    private HoplomachusScriptableObject hoploData;

    bool RDDelay = true;
    int RDDelayint = 0;

    void Start()
    {
        hoplomachusCombatRef = gameObject.GetComponent<HoplomachusCombat>();
        enemyMovementRef = gameObject.GetComponent<EnemyMovement>();
        hoplomachusHealthRef = gameObject.GetComponent<HoplomachusHealth>();
        characterPoiseRef = gameObject.GetComponent<CharacterPoise>();
        hoploData = hoplomachusCombatRef.HoploData;
        //StartCoroutine(CalculateDecision(0));
        characterPoiseRef.AddBreakPoiseEvent(PoiseBreak);
        characterPoiseRef.AddBreakPoiseEvent(() => enemyMovementRef.animHandleRef.SetStun(true));
        characterPoiseRef.AddBreakPoiseEvent(() => hoplomachusCombatRef.SwapWeapon(true));
        characterPoiseRef.AddBreakPoiseEvent(() =>
        {
            hoplomachusHealthRef.defaultColor = new Color(1.5f, 1.5f, 1.5f, 1);
            ParticleEffectController.Instance.CharAnimReciveDamage(null, enemyMovementRef.animHandleRef.GetMaterial(),
            characterPoiseRef.PoiseRecoverTime, new Color(1.5f, 1.5f, 1.5f, 1), hoplomachusHealthRef);
        });
        enemyMovementRef.StartCoroutine(ThrowSpearEvent());
        hoplomachusHealthRef.RDDelay.AddListener(ReciveDamageDelay);
        //hoplomachusHealthRef.RDDelay.AddListener(hoplomachusCombatRef.StopAllCoroutines);
    }

    public void ReciveDamageDelay()
    {
        if (RDDelay && !characterPoiseRef.IsPoise)
        {
            currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, hoplomachusCombatRef.TriggerActionFailure, this);
            AIAction aiAction = new AIAction();
            aiAction.onComplete.AddListener(ActionCompleted);
            aiAction.TakeAction(ai => StartCoroutine(CalculateDecision(0.1f)));
            currentAction = aiAction;
            RDDelay = false;
            Debug.Log("Recive Damage Sec " + 0.1f);
        }
    }

    public void PoiseBreak()
    {
        currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, hoplomachusCombatRef.TriggerActionFailure, this);
        AIAction aiAction = new AIAction();
        aiAction.onComplete.AddListener(ActionCompleted);
        aiAction.TakeAction(hoplomachusHealthRef.charPoiseRef.PoiseLock, hoplomachusHealthRef.charPoiseRef.PoiseRecoverTime);
        currentAction = aiAction;
        Debug.Log("Poise Break");
    }

    IEnumerator ThrowSpearEvent()
    {
        yield return new WaitForSeconds(1.1f);
        while (true)
        {
            if (!characterPoiseRef.IsPoise) yield return null;
            var dir = PlayerCharacterMovement.Instance.gameObject.transform.position - transform.position;
            var IDot = Vector2.Dot(dir.normalized, PlayerCharacterMovement.Instance.GetCurrDir().normalized);
            if (IDot > Mathf.Cos(hoploData.ThrowSpearAngle * Mathf.Deg2Rad))
            {
                var rand = Random.Range(0, 101);
                if (rand <= hoploData.ThrowSpearChance)
                {
                    currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, hoplomachusCombatRef.TriggerActionFailure, this);
                    AIAction aiAction = new AIAction();
                    aiAction.onComplete.AddListener(ActionCompleted);
                    aiAction.TakeAction(hoplomachusCombatRef.ThrowSpear);
                    currentAction = aiAction;
                    Debug.Log("Throw Spear");
                    yield break;
                }
                else
                {
                    float time = Random.Range(hoploData.ThrowSpearFailMinTime, hoploData.ThrowSpearFailMaxTime);
                    yield return new WaitForSeconds(time);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void PlayerEntered(GameObject hitObject, Collider2D collider)
    {

        playerClose = true;
        if (!hoplomachusHealthRef.charPoiseRef.IsPoise && !hoplomachusCombatRef.AttackLock && currentAction != null)
        {
            currentAction.StopAction(enemyMovementRef.TriggerActionFailed, hoplomachusCombatRef.TriggerActionFailure, this);
        }
    }
    public void PlayerExited(GameObject hitObject, Collider2D collider)
    {
        playerClose = false;
        if (!hoplomachusHealthRef.charPoiseRef.IsPoise && !hoplomachusCombatRef.AttackLock && currentAction != null)
        {
            currentAction.StopAction(enemyMovementRef.TriggerActionFailed, hoplomachusCombatRef.TriggerActionFailure, this);
        }
    }

    IEnumerator<WaitForSeconds> CalculateDecision(float delay)
    {
        yield return new WaitForSeconds(delay);
        int rand = UnityEngine.Random.Range(1, 101);
        if (playerClose)
        {
            if (rand <= hoploData.DefendChance)
            {
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                float time = Random.Range(hoploData.minDefTime,hoploData.maxDefTime);
                aiAction.TakeAction(hoplomachusCombatRef.DefendWithShield,time);
                currentAction = aiAction;
            }
            else if (rand <= hoploData.DefendChance + hoploData.AttackChance)
            {
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                aiAction.TakeAction(hoplomachusCombatRef.AttackTarget);
                currentAction = aiAction;
            }
        }
        else
        {
            if (rand <= hoploData.MoveTowordsPlayerChance)
            {
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                float trackTime = UnityEngine.Random.Range(hoploData.minMoveTowordsPlayer, hoploData.maxMoveTowordsPlayer);
                aiAction.TakeAction(enemyMovementRef.MoveToPlayer, trackTime);
                currentAction = aiAction;

                Debug.Log("tracking player for " + trackTime);
            }
            else if (rand <= hoploData.MoveTowordsPlayerChance + hoploData.MoveWaypointChance)
            {
                AIAction aiAction = new AIAction();
                aiAction.onComplete.AddListener(ActionCompleted);
                bool ReverseLocBoolX = enemyMovementRef.GetTarget().position.x - transform.position.x > 0;
                bool ReverseLocBoolY = enemyMovementRef.GetTarget().position.y - transform.position.y > 0;
                List<Vector3> posList = new List<Vector3>();
                int wayPointNum = UnityEngine.Random.Range(hoploData.minWaypoint, hoploData.maxWaypoint);
                int way = UnityEngine.Random.Range(1, 3);

                for (int i = 0; i < wayPointNum; i++)
                {
                    float longLenght = UnityEngine.Random.Range(hoploData.longMin, hoploData.longMax) * (i + 1) * 0.75f;
                    float shortLenght = UnityEngine.Random.Range(hoploData.shortMin, hoploData.shortMax) * (i + 1) * 1.4f;

                    if ((ReverseLocBoolX && ReverseLocBoolY) || (!ReverseLocBoolX && !ReverseLocBoolY))
                    {
                        if (way != 1)
                        {
                            float temp = longLenght;
                            longLenght = shortLenght;
                            shortLenght = temp;
                        }
                    }
                    else
                    {
                        if (way == 1)
                        {
                            float temp = longLenght;
                            longLenght = shortLenght;
                            shortLenght = temp;
                        }
                    }

                    longLenght = ReverseLocBoolX ? longLenght : longLenght * -1;
                    shortLenght = ReverseLocBoolY ? shortLenght : shortLenght * -1;

                    Vector3 vector3 = new Vector3(gameObject.transform.position.x + longLenght, gameObject.transform.position.y + shortLenght, gameObject.transform.position.z);
                    SetInBoundary(ref vector3);
                    if (CheckWaypoint(vector3))
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
                Debug.Log("Moving Random for " + wayPointNum);
            }
        }
        yield break;
    }

    void SetInBoundary(ref Vector3 vector3)
    {
        float x = Mathf.Clamp(vector3.x, GameController.Instance.CurrentArena.ArenaBoundary.BotX, GameController.Instance.CurrentArena.ArenaBoundary.TopX);
        float y = Mathf.Clamp(vector3.y, GameController.Instance.CurrentArena.ArenaBoundary.BotY, GameController.Instance.CurrentArena.ArenaBoundary.TopY);
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

        if (!RDDelay)
        {
            if (++RDDelayint == 2)
            {
                RDDelayint = 0;
                RDDelay = true;
            }
        }

    }
}
