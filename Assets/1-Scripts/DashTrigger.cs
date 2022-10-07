using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrigger : MonoBehaviour
{
    [HideInInspector]public EnemyCombat enemyCombat;

    private void Awake()
    {
        enemyCombat = GetComponentInParent<EnemyCombat>();
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        enemyCombat.LookAtPlayer();
        enemyCombat.PushSelfAttack();
    }
    private void OnDisable()
    {
        
    }
}
