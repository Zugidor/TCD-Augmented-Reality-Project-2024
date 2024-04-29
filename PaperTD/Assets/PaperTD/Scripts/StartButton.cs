using UnityEngine;

public class StartButton : MonoBehaviour
{
	int currentLvl = 0;
	bool begin = false;
	bool pathPresent = false;
	GameObject spawner;
	GameObject tower;
	readonly float margin = 50;
	readonly int font = 50;
	int towerLvl = 1;

	void Start()
	{
		spawner = GameObject.FindGameObjectWithTag("Spawner");
		tower = GameObject.FindGameObjectWithTag("Tower");
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
		else if (pathPresent && begin && towerLvl < 4)
		{
			if (GUILayout.Button($"Upgrade Lvl{towerLvl} Tower to Lvl{towerLvl + 1}"))
			{
				towerLvl++;
				tower.GetComponentInChildren<STT_Turret>().UpgradeTo(towerLvl);
			}
		}
		else if (pathPresent && begin && towerLvl == 4)
		{
			GUILayout.Button("Max tower level reached");
		}

		GUILayout.EndArea();
	}
}