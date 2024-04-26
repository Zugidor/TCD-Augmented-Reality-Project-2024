using UnityEngine;

// Class with Hard Coded 3 Node Path
public class EnemyNavScript : MonoBehaviour
{
	public float movementSpeed = 3.0f;
	float scale;
	string currentTargetTag = "MiddleNode";
	Transform target;

	// Start is called before the first frame update
	void Start()
	{
		target = GameObject.FindGameObjectWithTag(currentTargetTag).transform;
		scale = gameObject.transform.lossyScale.x;
		movementSpeed *= scale;
	}

	// Update is called once per frame
	void Update()
	{
		float distanceFromNode = (gameObject.transform.position - target.position).magnitude;
		if (distanceFromNode > 1 * scale)
		{
			Vector3 travelDirection = (target.position - gameObject.transform.position).normalized;
			gameObject.transform.position += movementSpeed * Time.deltaTime * travelDirection;
			gameObject.transform.LookAt(target);
		}
		else if (currentTargetTag == "MiddleNode")
		{
			currentTargetTag = "EndNode";
			target = GameObject.FindGameObjectWithTag(currentTargetTag).transform;
		}
	}
}
