using UnityEngine;

public class SpawningScript : MonoBehaviour
{
	public GameObject enemy;
	public float maxInterval;
	public float minInterval;
	public int maxEnemyCount;

	[HideInInspector]
	public bool begin = false;

	[HideInInspector]
	public bool pathPresent = false;

	float timer;

	void Start()
	{
		timer = Random.Range(minInterval, maxInterval);
	}

	void Update()
	{
		if (!pathPresent)
		{
			pathPresent = GameObject.FindGameObjectWithTag("StartNode") != null
				&& GameObject.FindGameObjectWithTag("MiddleNode") != null
				&& GameObject.FindGameObjectWithTag("EndNode") != null;
		}

		if (pathPresent && begin)
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
