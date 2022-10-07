using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject[] spawnPoints;

    void Start()
    {
        GameController.Instance.runStarted.AddListener(SpawnerStart);
    }

    public void SpawnerStart()
    {
        StartCoroutine(SpawnCo(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCo(float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);

        int randomSpawn = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyToSpawn, spawnPoints[randomSpawn].transform.position, Quaternion.identity);
        StartCoroutine(SpawnCo(Random.Range(3, 5)));
        yield break;
    }
}
