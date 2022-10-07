using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIAction 
{
    public UnityEvent<ActionResult> onComplete;


    public AIAction()
    {
        onComplete = new UnityEvent<ActionResult>();
    }

   
    public void TakeAction(Action<AIAction, Vector3[]> action, Vector3[] posList)
    {
        action(this, posList);
    }

    public void TakeAction(Action<AIAction, Vector3> action, Vector3 loc)
    {
        action(this, loc);
    }


    public void TakeAction(Action<AIAction, System.Func<bool>> action, System.Func<bool> GetPlayerClose)
    {
        action(this, GetPlayerClose);
    }

    public void TakeAction(Action<AIAction,float> action, float timeToMove)
    {
        action(this, timeToMove);
    }
    public void TakeAction(Action<AIAction> action)
    {
        action(this);
    }

    public void StopAction(Action<AIAction> movementStopAction, Action<AIAction> combatMovementAction,MonoBehaviour enemyAI)
    {
        movementStopAction(this);
        combatMovementAction(this);
        enemyAI.StopAllCoroutines();
        TriggerOncomplete(ActionResult.Failure);
    }
    public void StopActionAndStopDecsion(Action<AIAction> movementStopAction, Action<AIAction> combatMovementAction, MonoBehaviour enemyAI)
    {
        movementStopAction(this);
        combatMovementAction(this);
        enemyAI.StopAllCoroutines();
    }
    public void TriggerOncomplete(ActionResult result)
    {
        onComplete.Invoke(result);
    }
}
