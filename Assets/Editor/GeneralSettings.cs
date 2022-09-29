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
				DrawSettings((CharacterData)EnemyDesignerWindow.MageInfo);
				break;
			case SettingsType.ROGUE:
				DrawSettings((CharacterData)EnemyDesignerWindow.RogueInfo);
				break;
			case SettingsType.WARRIOR:
				DrawSettings((CharacterData)EnemyDesignerWindow.WarriorInfo);
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
		characterData.critChance = EditorGUILayout.FloatField(characterData.critChance);
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

	}
}
