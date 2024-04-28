using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneController : MonoBehaviour
{
	[HideInInspector]
	public ARAnchor groundAnchor;

	[HideInInspector]
	public ARPlane ground;

	ARPlaneManager planeMan;
	ARAnchorManager anchorMan;

	// Start is called before the first frame update
	void Start()
	{
		planeMan = GetComponent<ARPlaneManager>();
		planeMan.planesChanged += OnPlanesChanged;
		anchorMan = GetComponent<ARAnchorManager>();
	}

	public void OnPlanesChanged(ARPlanesChangedEventArgs changes)
	{
		foreach (ARPlane plane in changes.added)
		{
			// assuming first detected plane is the ground, only one we care about
			if (ground == null)
			{
				ground = plane;
				groundAnchor = anchorMan.AttachAnchor(ground, new Pose(ground.center, ground.transform.rotation));
				if (!Debug.isDebugBuild)
				{
					ground.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
				}
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
				// anchor is updated automatically
			}
		}

		foreach (ARPlane plane in changes.removed)
		{
			if (plane == ground)
			{
				ground = null;
				Destroy(groundAnchor); // we manually destroy ARAnchors, but not ARPlanes (see Unity docs)
				groundAnchor = null;
			}
		}
	}
}
