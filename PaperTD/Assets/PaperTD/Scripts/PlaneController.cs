using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneController : MonoBehaviour
{
	GameObject startN, midN, endN, tower;
	ARPlaneManager planeMan;
	ARPlane ground;

	// Start is called before the first frame update
	void Start()
	{
		planeMan = GetComponent<ARPlaneManager>();
		planeMan.planesChanged += OnPlanesChanged;
	}

	// Update is called once per frame
	void Update()
	{
		// only looking for and finding one of each
		if (startN == null)
		{
			startN = GameObject.FindGameObjectWithTag("StartNode");
		}
		if (midN == null)
		{
			midN = GameObject.FindGameObjectWithTag("MiddleNode");
		}
		if (endN == null)
		{
			endN = GameObject.FindGameObjectWithTag("EndNode");
		}
		if (tower == null)
		{
			tower = GameObject.FindGameObjectWithTag("Tower");
		}
	}

	public void OnPlanesChanged(ARPlanesChangedEventArgs changes)
	{
		foreach (ARPlane plane in changes.added)
		{
			if (ground == null)
			{
				ground = plane;
			}
			else
			{
				plane.gameObject.SetActive(false);
			}
		}

		foreach (ARPlane plane in changes.updated)
		{
			if (plane == ground)
			{
				float gy = ground.transform.position.y;
				float nodeOffset = (float)(gy + (1.4 * midN.transform.lossyScale.x));

				Debug.Log("Ground Y: " + gy + " Node Offset: " + nodeOffset + " Scale: " + midN.transform.lossyScale.x);

				Vector3 p = tower.transform.position;
				tower.transform.position.Set(p.x, gy, p.z);

				Debug.Log("Tower Y: " + tower.transform.position.y);

				p = startN.transform.position;
				startN.transform.position.Set(p.x, gy + nodeOffset, p.z);

				Debug.Log("Start Y: " + startN.transform.position.y);

				p = midN.transform.position;
				midN.transform.position.Set(p.x, gy + nodeOffset, p.z);

				Debug.Log("Middle Y: " + midN.transform.position.y);

				p = endN.transform.position;
				endN.transform.position.Set(p.x, gy + nodeOffset, p.z);

				Debug.Log("End Y: " + endN.transform.position.y);
			}
		}

		foreach (ARPlane plane in changes.removed)
		{
			if (plane == ground)
			{
				ground = null;
			}
		}
	}
}
