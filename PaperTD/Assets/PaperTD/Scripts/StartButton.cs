using UnityEngine;

public class StartButton : MonoBehaviour
{
	bool begin = false;
	bool pathPresent = false;
	GameObject spawner;
	readonly float margin = 50;
	readonly int font = 50;

	void Start()
	{
		spawner = GameObject.FindGameObjectWithTag("Spawner");
	}

	void Update()
	{
		pathPresent = spawner.GetComponent<SpawningScript>().pathPresent;
		begin = spawner.GetComponent<SpawningScript>().begin;
	}

	void OnGUI()
	{
		GUI.skin.button.fontSize = font;
		GUI.skin.label.fontSize = font;
		GUILayout.BeginArea(new Rect(margin * 2, margin, Screen.width - margin * 4, Screen.height - margin * 2));

		if (pathPresent && !begin)
		{
			if (GUILayout.Button("Start"))
			{
				spawner.GetComponent<SpawningScript>().begin = true;
			}
		}

		GUILayout.EndArea();
	}
}