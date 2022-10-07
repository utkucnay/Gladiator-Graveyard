using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagittariusAI : EnemyAI
{
    private bool playerClose;
    private SagittariusCombat sagittariusCombatRef;
    SagittariusHealth sagittariusHealth;
    CharacterPoise characterPoiseRef;

    SagittariusScriptableObject SagiData;

    void Start()
    {
        sagittariusCombatRef = gameObject.GetComponent<SagittariusCombat>();
        enemyMovementRef = gameObject.GetComponent<EnemyMovement>();
        characterPoiseRef = GetComponent<CharacterPoise>();
        sagittariusHealth = GetComponent<SagittariusHealth>();
        //StartCoroutine(CalculateDecision(0));
        characterPoiseRef.AddBreakPoiseEvent(PoiseBreak);
        characterPoiseRef.AddBreakPoiseEvent(() => enemyMovementRef.animHandleRef.SetStun(true));
        characterPoiseRef.AddBreakPoiseEvent(() => sagittariusCombatRef.SwapWeapon(true));
        characterPoiseRef.AddBreakPoiseEvent(() =>
        {
            sagittariusHealth.defaultColor = new Color(1.5f, 1.5f, 1.5f, 1);
            ParticleEffectController.Instance.CharAnimReciveDamage(null, enemyMovementRef.animHandleRef.GetMaterial(),
             characterPoiseRef.PoiseRecoverTime, new Color(1.5f, 1.5f, 1.5f, 1), sagittariusHealth);
        });
        SagiData = sagittariusCombatRef.SagiData;
        //characterPoiseRef.AddBreakPoiseEvent(() => enemyMovementRef.ChangePrevDir(enemyMovementRef.GetTarget().position - transform.position));
    }

    void Update()
    {
    }
    public void PoiseBreak()
    {
        currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, sagittariusCombatRef.TriggerActionFailure, this);
        AIAction aiAction = new AIAction();
        aiAction.onComplete.AddListener(ActionCompleted);
        aiAction.TakeAction(characterPoiseRef.PoiseLock, characterPoiseRef.PoiseRecoverTime);
        currentAction = aiAction;
        Debug.Log("Poise Break");
    }

    public void PlayerEntered(GameObject hitObject, Collider2D collider)
    {
        playerClose = true;
        Debug.Log("Stop Action");
        if (!characterPoiseRef.IsPoise && currentAction != null)
        {
            currentAction.StopAction(enemyMovementRef.TriggerActionFailed, sagittariusCombatRef.TriggerActionFailure, this);
        }
        //currentAction.StopAction(enemyMovementRef.TriggerActionFailed, sagittariusCombatRef.TriggerActionFailure,this);
        
    }
    public void PlayerExited(GameObject hitObject, Collider2D collider)
    {
        playerClose = false;
    }

    IEnumerator<WaitForSeconds> CalculateDecision(float delay)
    {
        yield return new WaitForSeconds(delay);
        int rand = UnityEngine.Random.Range(1, 101);
        if (playerClose)
        {
            AIAction aiAction = new AIAction();
            aiAction.onComplete.AddListener(ActionCompleted);
            aiAction.TakeAction(enemyMovementRef.DontMove);
            currentAction = aiAction;
            Debug.Log("Run Away From Player");

            while (playerClose)
            {

                Vector3 Dir = Vector3.Normalize(transform.position - enemyMovementRef.GetTarget().position);
                var Collider = Physics2D.OverlapCircleAll(transform.position, 1,LayerMask.GetMask("Wall"));
                if (Collider.Length != 0)
                {
                    if (Collider.Length < 2)
                    {
                        var dir = -Collider[0].offset;
                        if (dir.y > 10 || dir.y < -10)
                        {
                            Dir = new Vector3(1 * Mathf.Sign(Dir.x), 0.25f * Mathf.Sign(Dir.y), 0);
                        }
                        else if (dir.x > 10 || dir.x < -10)
                        {
                            Dir = new Vector3(0.25f * Mathf.Sign(Dir.x), 1 * Mathf.Sign(Dir.y) , 0);
                        }
                    }
                    else
                    {
                        currentAction.StopActionAndStopDecsion(enemyMovementRef.TriggerActionFailed, sagittariusCombatRef.TriggerActionFailure, this);
                        AIAction aiAction2 = new AIAction();
                        aiAction2.onComplete.AddListener(ActionCompleted);
                        aiAction2.TakeAction(sagittariusCombatRef.AttackTarget);
                        currentAction = aiAction2;
                        Debug.Log("ins");
                    }
                }
                if (!sagittariusCombatRef.amIDrawBow)
                {
                    Dir *= SagiData.RunFromPlayerSpeed;
                }
                else
                {
                    Dir *= SagiData.RunFromPlayerDrawSpeed;
                }

                

                enemyMovementRef.VelocityMoveToDir(Dir);

                yield return null;
            }

            currentAction.TriggerOncomplete(ActionResult.Success);

        }
        else
        {
            FarFromPlayer(rand);
        }
        yield break;
    }

    void FarFromPlayer(int rand)
    {

        if (rand <= SagiData.MoveWaypointChance)
        {
            AIAction aiAction = new AIAction();
            aiAction.onComplete.AddListener(ActionCompleted);
            List<Vector3> posList = new List<Vector3>();
            int wayPointNum = UnityEngine.Random.Range(SagiData.minWaypoint, SagiData.maxWaypoint);
            int way = UnityEngine.Random.Range(1, 3);

            for (int i = 0; i < wayPointNum; i++)
            {
                float radius = UnityEngine.Random.Range(SagiData.minRadius, SagiData.maxRadius);
                float angle = UnityEngine.Random.Range(SagiData.minAngle, SagiData.maxAngle) * (i+1);

                if (way == 1)
                {
                    angle *= -1;
                }

                Vector2 vector = (transform.position - enemyMovementRef.GetTarget().position).normalized;
                var Vector = rotate(vector, angle);

                Vector *= radius;

                Vector3 vector3 = new Vector3(enemyMovementRef.GetTarget().position.x + Vector.x, enemyMovementRef.GetTarget().position.y + Vector.y, gameObject.transform.position.z);
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
        else if (rand <= SagiData.MoveWaypointChance + SagiData.AttackChance)
        {
            AIAction aiAction = new AIAction();
            aiAction.onComplete.AddListener(ActionCompleted);
            aiAction.TakeAction(sagittariusCombatRef.AttackTarget);
            currentAction = aiAction;
            Debug.Log("ins");
        }
    }

    void SetInBoundary(ref Vector3 vector3)
    {
        vector3.x = Mathf.Clamp(vector3.x, GameController.Instance.CurrentArena.ArenaBoundary.BotX, GameController.Instance.CurrentArena.ArenaBoundary.TopX);
        vector3.y = Mathf.Clamp(vector3.y, GameController.Instance.CurrentArena.ArenaBoundary.BotY, GameController.Instance.CurrentArena.ArenaBoundary.TopY);
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        delta *= Mathf.Deg2Rad;
        v.x = v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta);
        v.y = v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta);
        return v;
    }

    protected override void ActionCompleted(ActionResult result)
    {
        Debug.Log("action completed with: " + result);
        switch (result)
        {
            case ActionResult.Success:
                StartCoroutine(CalculateDecision(1f));
                break;
            case ActionResult.Failure:
                StartCoroutine(CalculateDecision(0.1f));
                break;
            default:
                StartCoroutine(CalculateDecision(1f));
                break;
        }
       
    }
}
