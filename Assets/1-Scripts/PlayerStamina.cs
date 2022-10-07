using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : Singleton<PlayerStamina>
{
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float staminaRegenRate;
    [SerializeField]
    private float staminaRegenDelay;

    private float staminaRegenDelayTimer;

    private float currentStamina;
    private bool regenStamina;
    private bool staminaLock;
    private bool staminaRegenLock;
    public float reduceStaminaMultipler = 1;

    public BasePlayerAttributes playerAttributes;

    public float GetCurrentStamina()
    {
        return currentStamina;
    }
    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public override void RunStarted()
    {
        base.RunStarted();
        currentStamina = maxStamina;
        regenStamina = true;
        staminaLock = false;
        staminaRegenLock = false;
    }

    public override void Start()
    {
        base.Start();
    }
    void Update()
    {
        staminaRegenDelayTimer -= Time.deltaTime;
        if (staminaRegenDelayTimer > 0)
        {
            regenStamina = false;
        }
        else
        {
            regenStamina = true;
        }

        if (regenStamina && !staminaLock && !staminaRegenLock)
        {
            currentStamina += Time.deltaTime * staminaRegenRate;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        PlayerStaminaUI.Instance.SetSliderValue(currentStamina / maxStamina);
    }

    public bool ConsumeStamina(float staminaAmount)
    {
        
        if(staminaAmount > currentStamina)
        {
            return false;
        }
        currentStamina -= staminaAmount;
        staminaRegenDelayTimer = staminaRegenDelay;
        return true;
    }

   


    public void LockRegen()
    {
        staminaLock = true;
    }

    public void UnlockRegen()
    {
        staminaLock = false;
    }

    public void IncreaseMaxStamina(float limit)
    {
        maxStamina += limit;
    }
    public void SetReduceStaminaMultipler(float newMultipler)
    {
        reduceStaminaMultipler = newMultipler;
    }

    public void SetStaminaOnData()
    {
        currentStamina = playerAttributes.stam;
        maxStamina = playerAttributes.stam;
    }
}
