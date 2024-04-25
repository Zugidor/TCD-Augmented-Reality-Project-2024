using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SpawningScript : MonoBehaviour
{
    public GameObject enemy;
    public float maxInterval;
    public float minInterval;
    public int maxEnemyCount;

    float timer;

    void Start()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        // Check that a full enemy path is present
        bool pathPresent = GameObject.FindGameObjectWithTag("StartNode") != null
            && GameObject.FindGameObjectWithTag("MiddleNode") != null
            && GameObject.FindGameObjectWithTag("EndNode") != null;
        
        if (pathPresent)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = Random.Range(minInterval, maxInterval);
                
                int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
                if (enemyCount < maxEnemyCount)
                {
                    // Get the starting node
                    Vector3 spawnPosition = GameObject.FindGameObjectWithTag("StartNode").transform.position;
                    print(spawnPosition);
                    Instantiate(enemy, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
