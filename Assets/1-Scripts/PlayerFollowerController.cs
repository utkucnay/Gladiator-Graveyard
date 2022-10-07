using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowerController : MonoBehaviour
{
    [SerializeField] private GameObject playerFollower;
    [SerializeField]private GameObject centerPos;
    [SerializeField] private float range;

    private GameObject playerRef;
    private Transform playerTransformRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerTransformRef = playerRef.transform;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 vecToPlayer = playerTransformRef.position - centerPos.transform.position;
        Vector3 dir = vecToPlayer.normalized;
        Vector3 finalPos = dir * range;
        if(finalPos.magnitude > vecToPlayer.magnitude)
        {
            finalPos = vecToPlayer;
        }

        playerFollower.transform.position = finalPos;
    }
}
