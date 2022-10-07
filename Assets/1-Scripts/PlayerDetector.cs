using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public UnityEvent<GameObject, Collider2D> functionToCallOnPlayerEnter;
    public UnityEvent<GameObject, Collider2D> functionToCallOnPlayerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            functionToCallOnPlayerEnter?.Invoke(other.gameObject, other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            functionToCallOnPlayerExit?.Invoke(other.gameObject, other);
        }
    }
}
