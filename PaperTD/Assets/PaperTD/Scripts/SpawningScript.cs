using UnityEngine;

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
					Transform spawn = GameObject.FindGameObjectWithTag("StartNode").transform;
					print(spawn);
					Instantiate(enemy, spawn.position, Quaternion.identity, spawn);
				}
			}
		}
	}
}
