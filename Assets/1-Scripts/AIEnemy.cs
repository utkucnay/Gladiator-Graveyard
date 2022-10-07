using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshComponents;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    [SerializeField]
    public float AttackDistance;
    public Transform Target;
    public float AttackDelay;
    private bool Lock;
    CharacterRotator2D Cr;
    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Cr = GetComponent <CharacterRotator2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Lock = false;
    }

    private void Update()
    {
        agent.SetDestination(Target.position);
        AIRotate();
        AttackBegin();
    }

    private void AIRotate()
    {
        var Dir = agent.velocity.normalized;

        if (Dir.y > 0f)
        {
            if (Dir.x < 0.1f && Dir.x > -0.1f)
            {
                Cr.RotateCharacter(Direction.North);
            }
            else if (Dir.x > 0.1f)
            {
                Cr.RotateCharacter(Direction.NorthEast);
            }
            else
            {
                Cr.RotateCharacter(Direction.NorthWest);
            }
        }
        else if (Dir.y < 0f)
        {
            if (Dir.x < 0.1f && Dir.x > -0.1f)
            {
                Cr.RotateCharacter(Direction.South);
            }
            else if (Dir.x > 0.1f)
            {
                Cr.RotateCharacter(Direction.SouthEast);
            }
            else
            {
                Cr.RotateCharacter(Direction.SouthWest);
            }
        }
    }

    private bool isAttackRange()
    {
        if (AttackDistance >= Vector3.Distance(Target.transform.position,transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AttackBegin()
    {
        if (isAttackRange() && !Lock)
        {
            LockCharTrigger();
            StartCoroutine(Attack());
        }
    }
    private void LockCharTrigger()
    {
        Lock = !Lock;
        if (Lock)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    IEnumerator Attack()
    {
        //todo Attack
        Debug.Log("Attack");
        yield return new WaitForSeconds(AttackDelay);
        LockCharTrigger();
    }
}
