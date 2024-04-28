using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CameraLogger : MonoBehaviour
{
	[HideInInspector]
	public string debugText;

	ARCameraManager arCamMan;
	readonly float[] fpsPast300Frames = new float[300];
	float fps, avg, min, max;

	// Start is called before the first frame update
	void Start()
	{
		if (!Debug.isDebugBuild)
		{
			enabled = false;
			return;
		}
		arCamMan = GetComponent<ARCameraManager>();
	}

	// Update is called once per frame
	void Update()
	{
		// get fps
		fps = 1 / Time.deltaTime;

		// shift fpsPast300Frames and add new fps
		for (int i = 0; i < fpsPast300Frames.Length - 1; i++)
		{
			fpsPast300Frames[i] = fpsPast300Frames[i + 1];
		}
		fpsPast300Frames[^1] = fps;

		// get average, minimum, and maximum fps over the past ~10 seconds (at ~30fps)
		avg = 0;
		for (int i = 0; i < fpsPast300Frames.Length; i++)
		{
			avg += fpsPast300Frames[i];
		}
		avg /= fpsPast300Frames.Length;
		min = Mathf.Min(fpsPast300Frames);
		max = Mathf.Max(fpsPast300Frames);

		// log stats (camera resolution, screen resolution, target framerate, actual framerate)
		debugText = "Camera Resolution: " + arCamMan.currentConfiguration.Value.width.ToString()
			+ "x" + arCamMan.currentConfiguration.Value.height.ToString()
			+ "\nScreen Resolution: " + Screen.width.ToString() + "x" + Screen.height.ToString()
			+ "\nTarget FPS: " + arCamMan.currentConfiguration.Value.framerate.ToString()
			+ "\nActual FPS: " + fps.ToString()
			+ "\nAverage FPS: " + avg.ToString()
			+ "\nMinimum FPS: " + min.ToString()
			+ "\nMaximum FPS: " + max.ToString();
		//Debug.Log(debugText);
	}
}
