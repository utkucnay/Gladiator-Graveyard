using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    public float maxHealth;
    protected float currentHealth;
    public Coroutine PreAnimCorRef;
    protected IAnimationController animationController;
    [HideInInspector]public Color defaultColor = Color.white;
    
    public virtual void Start()
    {
        currentHealth = maxHealth;
        animationController = GetComponent<IAnimationController>();
        
    }

    public virtual void ReceiveDamage(HitInfo hitInfo)
    {
        currentHealth -= hitInfo.damage;
        
    }
    public virtual void Update()
    {
        
    }
}
