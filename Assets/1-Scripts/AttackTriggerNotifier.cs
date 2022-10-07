using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackTriggerNotifier : MonoBehaviour
{
    public LayerMask layersToCheck;
    public UnityEvent<GameObject, Collider2D> functionToCallOnHit;

    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEvent(UnityAction<GameObject, Collider2D> Action)
    {
        functionToCallOnHit.AddListener(Action);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (layersToCheck == (layersToCheck | (1 << collider.gameObject.layer)))
        {
            functionToCallOnHit?.Invoke(collider.gameObject, collider);
        }
    }

   

}
