using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Types;

public class GeneralSettings : EditorWindow {
	public enum SettingsType {
		MAGE,
		WARRIOR,
		ROGUE
	}

	private static SettingsType dataSetting;
	private static GeneralSettings window;

	public static void OpenWindow(SettingsType setting) {
		dataSetting = setting;
		window = (GeneralSettings)GetWindow(typeof(GeneralSettings));
		window.minSize = new Vector2(250, 200);
		window.Show();
	}

	private void OnGUI() {
		switch (dataSetting) {
			case SettingsType.MAGE:
				DrawSettings(EnemyDesignerWindow.MageInfo);
				break;
			case SettingsType.ROGUE:
				DrawSettings(EnemyDesignerWindow.RogueInfo);
				break;
			case SettingsType.WARRIOR:
				DrawSettings(EnemyDesignerWindow.WarriorInfo);
				break;
		}
	}

	private void DrawSettings(CharacterData characterData) {
		// prefab
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab");
		characterData.prefab = (GameObject)EditorGUILayout.ObjectField(characterData.prefab, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

		// name
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Name");
		characterData.name = EditorGUILayout.TextField(characterData.name);
		EditorGUILayout.EndHorizontal();

		// max health
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Health");
		characterData.maxHealth = EditorGUILayout.FloatField(characterData.maxHealth);
		EditorGUILayout.EndHorizontal();

		// max energy
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Energy");
		characterData.maxEnergy = EditorGUILayout.FloatField(characterData.maxEnergy);
		EditorGUILayout.EndHorizontal();

		// power
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Power");
		characterData.power = EditorGUILayout.Slider(characterData.power, 0, 100);
		EditorGUILayout.EndHorizontal();

		// crit chance
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Crit Chance %");
		characterData.critChance = EditorGUILayout.Slider(characterData.critChance, 0, characterData.power);
		EditorGUILayout.EndHorizontal();

		if (characterData.prefab == null)
			EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Warning);
		else if (characterData.name == null || characterData.name.Length < 1)
			EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
		else if (GUILayout.Button("Finish and Save", GUILayout.Height(30))) {
			SaveCharacterData();
			window.Close();
		}
	}

	private void SaveCharacterData() {
		string prefabPath;
		string newPrefabPath = "Assets/Prefabs/Characters/";
		string dataPath = "Assets/Resources/CharacterData/Data/";

		switch (dataSetting) {
			case SettingsType.MAGE:
				dataPath += "Mage/" + EnemyDesignerWindow.MageInfo.name + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.MageInfo, dataPath);

				newPrefabPath += "Mage/" + EnemyDesignerWindow.MageInfo.name + ".prefab";
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.MageInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject magePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

				if (!magePrefab.GetComponent<Mage>())
					magePrefab.AddComponent(typeof(Mage));

				magePrefab.GetComponent<Mage>().mageData = EnemyDesignerWindow.MageInfo;

				break;
			case SettingsType.ROGUE:
				dataPath += "Rogue/" + EnemyDesignerWindow.RogueInfo.name + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.RogueInfo, dataPath);

				newPrefabPath += "Rogue/" + EnemyDesignerWindow.RogueInfo.name + ".prefab";
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.RogueInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject roguePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

				if (!roguePrefab.GetComponent<Rogue>())
					roguePrefab.AddComponent(typeof(Rogue));

				roguePrefab.GetComponent<Rogue>().rogueData = EnemyDesignerWindow.RogueInfo;

				break;
			case SettingsType.WARRIOR:
				dataPath += "Warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.WarriorInfo, dataPath);

				newPrefabPath += "Warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".prefab";
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.WarriorInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject warriorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));

				if (!warriorPrefab.GetComponent<Warrior>())
					warriorPrefab.AddComponent(typeof(Warrior));

				warriorPrefab.GetComponent<Warrior>().warriorData = EnemyDesignerWindow.WarriorInfo;

				break;
		}
	}
}
