using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Types;

public class EnemyEditorWindow : EditorWindow {
	public enum SettingsType {
		MAGE,
		WARRIOR,
		ROGUE
	}

	private static SettingsType dataSetting;

	private Texture2D headerSectionTexture;
	private Texture2D prefabSelectionSectionTexture;
	private Texture2D settingsSectionTexture;
	private Texture2D settingsIconTexture;

	private Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

	private Rect headerSection;
	private Rect prefabSelectionSection;
	private Rect settingsIconSection;
	private Rect settingsSection;

	private GUISkin skin;

	private GameObject prefab;
	private CharacterData characterData;
	private string characterName;

	private float prefabSelectionSectionHeight = 60;
	private float iconSize = 80;
	private bool isLoaded = false;

	[MenuItem("Window/Enemy Editor")]
	private static void OpenWindow() {
		EnemyEditorWindow window = (EnemyEditorWindow)GetWindow(typeof(EnemyEditorWindow));
		window.minSize = new Vector2(250, 300);
		window.Show();
	}

	private void OnEnable() {
		InitTextures();
		skin = Resources.Load<GUISkin>("GUIStyles/EnemyDesignerSkin");
	}

	private void InitTextures() {
		headerSectionTexture = new Texture2D(1, 1);
		headerSectionTexture.SetPixel(0, 0, headerSectionColor);
		headerSectionTexture.Apply();

		prefabSelectionSectionTexture = new Texture2D(1, 1);
		prefabSelectionSectionTexture.SetPixel(0, 0, headerSectionColor);
		prefabSelectionSectionTexture.Apply();
	}

	private void InitSettings() {
		switch (dataSetting) {
			case SettingsType.MAGE: // mage
				settingsSectionTexture = Resources.Load<Texture2D>("icons/editorMageGradient");
				settingsIconTexture = Resources.Load<Texture2D>("icons/editorMageIcon");
				break;
			case SettingsType.ROGUE: // rogue
				settingsSectionTexture = Resources.Load<Texture2D>("icons/editorRogueGradient");
				settingsIconTexture = Resources.Load<Texture2D>("icons/editorRogueIcon");
				break;
			case SettingsType.WARRIOR: // warrior
				settingsSectionTexture = Resources.Load<Texture2D>("icons/editorWarriorGradient");
				settingsIconTexture = Resources.Load<Texture2D>("icons/editorWarriorIcon");
				break;
		}
	}

	private void OnGUI() {
		DrawLayouts();
		DrawHeader();
		DrawPrefabSelection();

		if (isLoaded)
			DrawSettings();
	}

	private void DrawLayouts() {
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50;

		prefabSelectionSection.x = 0;
		prefabSelectionSection.y = 50;
		prefabSelectionSection.width = Screen.width;
		prefabSelectionSection.height = prefabSelectionSectionHeight;

		GUI.DrawTexture(headerSection, headerSectionTexture);
		GUI.DrawTexture(prefabSelectionSection, prefabSelectionSectionTexture);

		if (isLoaded) {
			settingsSection.x = 0;
			settingsSection.y = headerSection.height + prefabSelectionSection.height;
			settingsSection.width = Screen.width;
			settingsSection.height = Screen.height - headerSection.height - prefabSelectionSection.height;

			settingsIconSection.x = (settingsSection.width / 2f) - iconSize / 2f;
			settingsIconSection.y = settingsSection.y;
			settingsIconSection.width = iconSize;
			settingsIconSection.height = iconSize;

			GUI.DrawTexture(settingsSection, settingsSectionTexture);
			GUI.DrawTexture(settingsIconSection, settingsIconTexture);
		}
	}

	private void DrawHeader() {
		GUILayout.BeginArea(headerSection);

		GUILayout.Label("Enemy Editor", skin.GetStyle("Header"));

		GUILayout.EndArea();
	}

	private void DrawPrefabSelection() {
		bool isSelected = false;

		GUILayout.BeginArea(prefabSelectionSection);
		EditorGUILayout.BeginHorizontal();

		prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);

		if (prefab == null) {
			prefabSelectionSectionHeight = 60;
			isSelected = false;
		}
		else {
			prefabSelectionSectionHeight = 18;
			isSelected = true;

			if (GUILayout.Button("Load", GUILayout.Height(18))) {
				if (prefab.GetComponent<Mage>() != null) {
					dataSetting = SettingsType.MAGE;
					InitSettings();
					characterData = prefab.GetComponent<Mage>().mageData;
				}
				else if (prefab.GetComponent<Rogue>() != null) {
					dataSetting = SettingsType.ROGUE;
					InitSettings();
					characterData = prefab.GetComponent<Rogue>().rogueData;
				}
				else if (prefab.GetComponent<Warrior>() != null) {
					dataSetting = SettingsType.WARRIOR;
					InitSettings();
					characterData = prefab.GetComponent<Warrior>().warriorData;
				}

				characterName = characterData.name;
				isLoaded = true;
			}
		}

		EditorGUILayout.EndHorizontal();

		if (!isSelected) {
			EditorGUILayout.HelpBox("A [Prefab] needs to be selected before it can be edited.", MessageType.Warning);
			isSelected = false;
		}

		GUILayout.EndArea();
	}

	private void DrawSettings() {
		string style = "";

		GUILayout.BeginArea(settingsSection);

		GUILayout.Space(iconSize);

		switch (dataSetting) {
			case SettingsType.MAGE:
				style = "Mage";
				 
				GUILayout.Label("Mage", skin.GetStyle(style + "Header"));

				// damage type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Damage", skin.GetStyle(style + "Field"));
				((MageData)characterData).damageType = (MageDamageType)EditorGUILayout.EnumPopup(((MageData)characterData).damageType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((MageData)characterData).weaponType = (MageWeaponType)EditorGUILayout.EnumPopup(((MageData)characterData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
			case SettingsType.ROGUE:
				style = "Rogue";

				GUILayout.Label("Rogue", skin.GetStyle(style + "Header"));

				// strategy type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Strategy", skin.GetStyle(style + "Field"));
				((RogueData)characterData).strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(((RogueData)characterData).strategyType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((RogueData)characterData).weaponType = (RogueWeaponType)EditorGUILayout.EnumPopup(((RogueData)characterData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
			case SettingsType.WARRIOR:
				style = "Warrior";

				GUILayout.Label("Warrior", skin.GetStyle(style + "Header"));

				// class type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Class", skin.GetStyle(style + "Field"));
				((WarriorData)characterData).classType = (WarriorClassType)EditorGUILayout.EnumPopup(((WarriorData)characterData).classType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((WarriorData)characterData).weaponType = (WarriorWeaponType)EditorGUILayout.EnumPopup(((WarriorData)characterData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
		}

		// prefab
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab", skin.GetStyle(style + "Field"));
		characterData.prefab = (GameObject)EditorGUILayout.ObjectField(characterData.prefab, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

		// name
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Name", skin.GetStyle(style + "Field"));
		characterData.name = EditorGUILayout.TextField(characterData.name);
		EditorGUILayout.EndHorizontal();

		// max health
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Health", skin.GetStyle(style + "Field"));
		characterData.maxHealth = EditorGUILayout.FloatField(characterData.maxHealth);
		EditorGUILayout.EndHorizontal();

		// max energy
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Energy", skin.GetStyle(style + "Field"));
		characterData.maxEnergy = EditorGUILayout.FloatField(characterData.maxEnergy);
		EditorGUILayout.EndHorizontal();

		// power
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Power", skin.GetStyle(style + "Field"));
		characterData.power = EditorGUILayout.Slider(characterData.power, 0, 100);
		EditorGUILayout.EndHorizontal();

		// crit chance
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Crit Chance %", skin.GetStyle(style + "Field"));
		characterData.critChance = EditorGUILayout.Slider(characterData.critChance, 0, characterData.power);
		EditorGUILayout.EndHorizontal();

		if (characterData.prefab == null)
			EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Warning);
		else if (characterData.name == null || characterData.name.Length < 1)
			EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
		else if (GUILayout.Button("Edit", GUILayout.Height(30)))
			SaveCharacterData();

		GUILayout.EndArea();
	}

	private void SaveCharacterData() {
		string prefabPath = AssetDatabase.GetAssetPath(prefab);
		string newPrefabPath = "Assets/Prefabs/Characters/";
		string dataPath = "Assets/Resources/CharacterData/Data/";
		string newDataPath = "Assets/Resources/CharacterData/Data/";

		switch (dataSetting) {
			case SettingsType.MAGE:
				newPrefabPath += "Mage/" + characterData.name + ".prefab";
				dataPath += "Mage/" + characterName + ".asset";
				newDataPath += "Mage/" + characterData.name + ".asset";

				MageData mageData = (MageData)AssetDatabase.LoadAssetAtPath(dataPath, typeof(MageData));
				mageData = (MageData)characterData;
				System.IO.File.Move(dataPath, newDataPath);
				System.IO.File.Move(dataPath + ".meta", newDataPath + ".meta");

				GameObject magePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
				magePrefab.GetComponent<Mage>().mageData = (MageData)characterData;
				System.IO.File.Move(prefabPath, newPrefabPath);
				EditorUtility.SetDirty(magePrefab);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				break;
			case SettingsType.ROGUE:
				dataPath += "Rogue/" + EnemyDesignerWindow.RogueInfo.name + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.RogueInfo, dataPath);

				newPrefabPath += "Rogue/" + EnemyDesignerWindow.RogueInfo.name + ".prefab";
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.RogueInfo.prefab);

				GameObject roguePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));

				if (!roguePrefab.GetComponent<Rogue>())
					roguePrefab.AddComponent(typeof(Rogue));

				roguePrefab.GetComponent<Rogue>().rogueData = EnemyDesignerWindow.RogueInfo;

				System.IO.File.Move(prefabPath, newPrefabPath);

				break;
			case SettingsType.WARRIOR:
				dataPath += "Warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.WarriorInfo, dataPath);

				newPrefabPath += "Warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".prefab";
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.WarriorInfo.prefab);

				GameObject warriorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));

				if (!warriorPrefab.GetComponent<Warrior>())
					warriorPrefab.AddComponent(typeof(Warrior));

				warriorPrefab.GetComponent<Warrior>().warriorData = EnemyDesignerWindow.WarriorInfo;

				System.IO.File.Move(prefabPath, newPrefabPath);

				break;
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
}
