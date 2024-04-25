using UnityEngine;

// Class with Hard Coded 3 Node Path
public class EnemyNavScript : MonoBehaviour
{
	public float movementSpeed = 5;
	float scale;
	string currentTargetTag = "MiddleNode";
	Vector3 targetPos;

	// Start is called before the first frame update
	void Start()
	{
		targetPos = GameObject.FindGameObjectWithTag(currentTargetTag).transform.position;
		scale = gameObject.transform.localScale.x;
		movementSpeed *= scale;
	}

	// Update is called once per frame
	void Update()
	{
		float distanceFromNode = (gameObject.transform.position - targetPos).magnitude;
		if (distanceFromNode > 0.1 * scale)
		{
			Vector3 travelDirection = (targetPos - gameObject.transform.position).normalized;
			gameObject.transform.position += movementSpeed * Time.deltaTime * travelDirection;
			Vector3 forwardOnPlane = gameObject.transform.forward;
			forwardOnPlane.y = 0;
			float angle = Vector3.Angle(forwardOnPlane.normalized, travelDirection);
			gameObject.transform.Rotate(new Vector3(0, 1, 0), angle);
		}
		else if (currentTargetTag == "MiddleNode")
		{
			currentTargetTag = "EndNode";
			targetPos = GameObject.FindGameObjectWithTag(currentTargetTag).transform.position;
		}
	}
}
