using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected AIAction currentAction;
    protected EnemyMovement enemyMovementRef;
    public void Move(Dir dir)
    {
        AIAction aiAction = new AIAction();
        aiAction.onComplete.AddListener(ActionCompleted);
        Vector3 vectors = Vector3.zero;
        switch (dir)
        {
            case Dir.up:
                vectors = Vector3.up * 3.5f;
                break;
            case Dir.down:
                vectors = Vector3.down * 3.5f;
                break;
            case Dir.left:
                vectors = Vector3.left * 3.5f;
                break;
            case Dir.right:
                vectors = Vector3.right * 3.5f;
                break;
            default:
                
                break;
        }
        StartCoroutine(Move(aiAction,vectors));
    }

    IEnumerator Move(AIAction aiAction, Vector3 vectors)
    {
        yield return null;
        aiAction.TakeAction(enemyMovementRef.MovePositionGameBegin, vectors);
        currentAction = aiAction;
    }
    protected virtual void ActionCompleted(ActionResult result)
    {
        
    }

    protected bool CheckWaypoint(Vector3 vector)
    {
        if(Physics2D.OverlapCircle(vector, 0.1f,LayerMask.GetMask("Wall")) == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
