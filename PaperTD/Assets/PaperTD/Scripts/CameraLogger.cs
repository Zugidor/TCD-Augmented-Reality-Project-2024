using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraLogger : MonoBehaviour
{
	ARCameraManager arCamMan;
	readonly float[] fpsPast300Frames = new float[300];
	float fps, avg;

	// Start is called before the first frame update
	void Start()
	{
		arCamMan = GetComponent<ARCameraManager>();
	}

	// Update is called once per frame
	void Update()
	{
		// if development build is not enabled, return
		if (!Debug.isDebugBuild)
		{
			return;
		}

		// get fps
		fps = 1 / Time.deltaTime;

		// shift fpsPast300Frames and add new fps
		for (int i = 0; i < fpsPast300Frames.Length - 1; i++)
		{
			fpsPast300Frames[i] = fpsPast300Frames[i + 1];
		}
		fpsPast300Frames[^1] = fps;

		// get average fps
		avg = 0;
		for (int i = 0; i < fpsPast300Frames.Length; i++)
		{
			avg += fpsPast300Frames[i];
		}
		avg /= fpsPast300Frames.Length;

		// log stats (camera resolution, screen resolution, target framerate, actual framerate)
		Debug.Log("Camera Resolution: " + arCamMan.currentConfiguration.Value.width.ToString()
			+ "x" + arCamMan.currentConfiguration.Value.height.ToString()
			+ "\nScreen Resolution: " + Screen.width.ToString() + "x" + Screen.height.ToString()
			+ "\nTarget FPS: " + arCamMan.currentConfiguration.Value.framerate.ToString()
			+ "\nActual FPS: " + fps.ToString()
			+ "\nAverage FPS: " + avg.ToString());
	}
}
