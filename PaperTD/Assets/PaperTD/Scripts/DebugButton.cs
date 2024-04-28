using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DebugButton : MonoBehaviour
{
	readonly float margin = 100;
	readonly int font = 50;
	bool debugMode = false;

	GameObject plane, start, mid, end;
	PlaneController planeCon;
	CameraLogger camLog;
	Text UIlog;

	void Start()
	{
		UIlog = GameObject.FindGameObjectWithTag("DebugLog").GetComponent<Text>();
		if (!Debug.isDebugBuild)
		{
			UIlog.gameObject.SetActive(false);
			enabled = false; // disable script if not in development build, will not call Update() or OnGUI()
			return;
		}
		debugMode = true;
		planeCon = gameObject.GetComponent<PlaneController>();
		camLog = gameObject.GetComponentInChildren<CameraLogger>();
	}

	// Assign plane and nodes when they appear and control their mesh renderers with debug mode boolean.
	// Also enable/disable UI text log and update its text every frame. 
	// Use LateUpdate() to ensure text is updated using the latest data.
	void LateUpdate()
	{
		if (debugMode)
		{
			UIlog.text = camLog.debugText;
		}
		else
		{
			UIlog.text = "";
		}

		if (plane == null && planeCon.ground != null)
		{
			plane = planeCon.ground.gameObject;
		}
		else if (plane != null)
		{
			if (debugMode)
			{
				plane.GetComponent<ARPlaneMeshVisualizer>().enabled = true;
			}
			else
			{
				plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
			}
		}

		if (start == null)
		{
			start = GameObject.FindGameObjectWithTag("StartNode");
		}
		else
		{
			if (debugMode)
			{
				start.GetComponent<MeshRenderer>().enabled = true;
			}
			else
			{
				start.GetComponent<MeshRenderer>().enabled = false;
			}
		}

		if (mid == null)
		{
			mid = GameObject.FindGameObjectWithTag("MiddleNode");
		}
		else
		{
			if (debugMode)
			{
				mid.GetComponent<MeshRenderer>().enabled = true;
			}
			else
			{
				mid.GetComponent<MeshRenderer>().enabled = false;
			}
		}

		if (end == null)
		{
			end = GameObject.FindGameObjectWithTag("EndNode");
		}
		else
		{
			if (debugMode)
			{
				end.GetComponent<MeshRenderer>().enabled = true;
			}
			else
			{
				end.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	// Simple button that controls debug mode boolean
	void OnGUI()
	{
		GUI.skin.button.fontSize = font;
		GUI.skin.label.fontSize = font;
		GUILayout.BeginArea(new Rect(margin, Screen.height - margin, Screen.width - margin * 2, Screen.height - margin));

		if (debugMode)
		{
			if (GUILayout.Button("Debug Mode: ON"))
			{
				debugMode = false;
			}
		}
		else
		{
			if (GUILayout.Button("Debug Mode: OFF"))
			{
				debugMode = true;
			}
		}

		GUILayout.EndArea();
	}
}