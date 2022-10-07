using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPoise : MonoBehaviour
{
    public float CurrentPoise { get; private set; }
    
    protected float PoiseAddbySecond;
    public int MaxPoise;
    [HideInInspector]public int PoiseRecoverTime;
    public bool IsPoise { get; private set; }

    EnemyMovement enemyMovementRef;
    EnemyCombat enemyCombatref;
    EnemyHealth enemyHealthRef;

    UnityEvent BreakPoise = new UnityEvent();

    public UnityAction<UnityAction> AddBreakPoiseEvent;
    public UnityAction StartBreakPoiseEvent;

    private void Awake()
    {
        AddBreakPoiseEvent = Action => { BreakPoise.AddListener(Action); };
        StartBreakPoiseEvent = () => { if (BreakPoise != null) BreakPoise.Invoke(); };
    }

    protected virtual void Start()
    {
        CurrentPoise = MaxPoise;
        IsPoise = false;
        enemyMovementRef = GetComponent<EnemyMovement>();
        enemyCombatref = GetComponent<EnemyCombat>();
        enemyHealthRef = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AddPoisebyTime(Time.deltaTime);
    }

    public virtual void ReducePoise(float number)
    {
        CurrentPoise -= number;
        if (CurrentPoise <= 0 && !IsPoise && !enemyHealthRef.IsDead)
        {
            IsPoise = true;
            enemyCombatref.SwapWeapon(true);
            enemyCombatref.StopAllCoroutines();
            StartBreakPoiseEvent();
            ParticleEffectController.Instance.PlayStunParticle(transform,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f, gameObject.transform.position.z));
        }
    }

    public void AddPoisebyTime(float deltaTime)
    {
        CurrentPoise += deltaTime * PoiseAddbySecond;
        if (CurrentPoise > MaxPoise)
        {
            CurrentPoise = MaxPoise;
        }
    }

    public void ResetPoise(AIAction action, AnimHandle animHandleRef)
    {
        CurrentPoise = MaxPoise;
        IsPoise = false;
        enemyHealthRef.defaultColor = Color.white;
        animHandleRef.SetStun(false);
        action.TriggerOncomplete(ActionResult.Success);
    }

    public void PoiseLock(AIAction action, float Time)
    {
        StartCoroutine(PoiseLockCor(action, Time));
    }
    public IEnumerator<WaitForSeconds> PoiseLockCor(AIAction action, float Time)
    {
        yield return new WaitForSeconds(Time);
        ResetPoise(action, enemyMovementRef.animHandleRef);
    }
}
