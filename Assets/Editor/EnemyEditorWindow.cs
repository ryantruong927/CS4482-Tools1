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
	private CharacterData editedData;
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

	// Settings are intialized after loading because I decided to style the window based on the character type.
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

		// only draw settings if a character was selected and loaded
		if (isLoaded)
			DrawSettings();
	}

	private void DrawLayouts() {
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = position.width;
		headerSection.height = 50;

		prefabSelectionSection.x = 0;
		prefabSelectionSection.y = 50;
		prefabSelectionSection.width = position.width;
		prefabSelectionSection.height = prefabSelectionSectionHeight;

		GUI.DrawTexture(headerSection, headerSectionTexture);
		GUI.DrawTexture(prefabSelectionSection, prefabSelectionSectionTexture);

		// no need to draw settings layout if it is not ready yet
		if (isLoaded) {
			// settings must be resized based on prefab selection section due to popup
			settingsSection.x = 0;
			settingsSection.y = headerSection.height + prefabSelectionSection.height;
			settingsSection.width = position.width;
			settingsSection.height = position.height - headerSection.height - prefabSelectionSection.height;

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
		bool isSelected;

		GUILayout.BeginArea(prefabSelectionSection);
		EditorGUILayout.BeginHorizontal();

		prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);

		// if prefab isn't selected, resize section to allow for popup
		if (prefab == null) {
			prefabSelectionSectionHeight = 60;
			isSelected = false;
		}
		// else, resize section to remove extra space
		else {
			prefabSelectionSectionHeight = 18;
			isSelected = true;

			if (GUILayout.Button("Load", GUILayout.Height(18))) {
				// check what type of character is selected based on what data they hold

				if (prefab.GetComponent<Mage>() != null) {
					dataSetting = SettingsType.MAGE;
					characterData = prefab.GetComponent<Mage>().mageData;
				}
				else if (prefab.GetComponent<Rogue>() != null) {
					dataSetting = SettingsType.ROGUE;
					characterData = prefab.GetComponent<Rogue>().rogueData;
				}
				else if (prefab.GetComponent<Warrior>() != null) {
					dataSetting = SettingsType.WARRIOR;
					characterData = prefab.GetComponent<Warrior>().warriorData;
				}

				// clone the data to prevent editing the data before clicking edit
				editedData = cloneData(characterData);
				characterName = editedData.name;
				InitSettings();
				isLoaded = true;
			}
		}

		EditorGUILayout.EndHorizontal();

		// placed afterward to not be included in same horizontal as field/button
		if (!isSelected)
			EditorGUILayout.HelpBox("A [Prefab] needs to be selected before it can be edited.", MessageType.Warning);

		GUILayout.EndArea();
	}

	// Similar to GeneralSettings.DrawSettings()
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
				((MageData)editedData).damageType = (MageDamageType)EditorGUILayout.EnumPopup(((MageData)editedData).damageType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((MageData)editedData).weaponType = (MageWeaponType)EditorGUILayout.EnumPopup(((MageData)editedData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
			case SettingsType.ROGUE:
				style = "Rogue";

				GUILayout.Label("Rogue", skin.GetStyle(style + "Header"));

				// strategy type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Strategy", skin.GetStyle(style + "Field"));
				((RogueData)editedData).strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(((RogueData)editedData).strategyType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((RogueData)editedData).weaponType = (RogueWeaponType)EditorGUILayout.EnumPopup(((RogueData)editedData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
			case SettingsType.WARRIOR:
				style = "Warrior";

				GUILayout.Label("Warrior", skin.GetStyle(style + "Header"));

				// class type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Class", skin.GetStyle(style + "Field"));
				((WarriorData)editedData).classType = (WarriorClassType)EditorGUILayout.EnumPopup(((WarriorData)editedData).classType);
				EditorGUILayout.EndHorizontal();

				// weapon type
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Weapon", skin.GetStyle(style + "Field"));
				((WarriorData)editedData).weaponType = (WarriorWeaponType)EditorGUILayout.EnumPopup(((WarriorData)editedData).weaponType);
				EditorGUILayout.EndHorizontal();

				break;
		}

		// prefab
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab", skin.GetStyle(style + "Field"));
		editedData.prefab = (GameObject)EditorGUILayout.ObjectField(editedData.prefab, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

		// name
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Name", skin.GetStyle(style + "Field"));
		editedData.name = EditorGUILayout.TextField(editedData.name);
		EditorGUILayout.EndHorizontal();

		// max health
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Health", skin.GetStyle(style + "Field"));
		editedData.maxHealth = EditorGUILayout.FloatField(editedData.maxHealth);
		EditorGUILayout.EndHorizontal();

		// max energy
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Energy", skin.GetStyle(style + "Field"));
		editedData.maxEnergy = EditorGUILayout.FloatField(editedData.maxEnergy);
		EditorGUILayout.EndHorizontal();

		// power
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Power", skin.GetStyle(style + "Field"));
		editedData.power = EditorGUILayout.Slider(editedData.power, 0, 100);
		EditorGUILayout.EndHorizontal();

		// crit chance
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Crit Chance %", skin.GetStyle(style + "Field"));
		editedData.critChance = EditorGUILayout.Slider(editedData.critChance, 0, editedData.power);
		EditorGUILayout.EndHorizontal();

		if (editedData.prefab == null)
			EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Warning);
		else if (editedData.name == null || editedData.name.Length < 1)
			EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
		else if (GUILayout.Button("Edit", GUILayout.Height(30)))
			SaveCharacterData();

		GUILayout.EndArea();
	}

	private void SaveCharacterData() {
		string prefabPath = AssetDatabase.GetAssetPath(prefab);
		string newPrefabPath = "";
		string dataPath = AssetDatabase.GetAssetPath(characterData);
		string newDataPath = "";

		switch (dataSetting) {
			case SettingsType.MAGE:
				newPrefabPath = editedData.name + ".prefab";
				newDataPath = editedData.name + ".asset";

				// edit and save the data
				MageData mageData = (MageData)AssetDatabase.LoadAssetAtPath(dataPath, typeof(MageData));
				cloneData(editedData, mageData);
				EditorUtility.SetDirty(mageData);

				characterData = mageData;

				break;
			case SettingsType.ROGUE:
				newPrefabPath = editedData.name + ".prefab";
				newDataPath = editedData.name + ".asset";

				RogueData rogueData = (RogueData)AssetDatabase.LoadAssetAtPath(dataPath, typeof(RogueData));
				cloneData(editedData, rogueData);
				EditorUtility.SetDirty(rogueData);

				characterData = rogueData;

				break;
			case SettingsType.WARRIOR:
				newPrefabPath = editedData.name + ".prefab";
				newDataPath = editedData.name + ".asset";

				WarriorData warriorData = (WarriorData)AssetDatabase.LoadAssetAtPath(dataPath, typeof(WarriorData));
				cloneData(editedData, warriorData);
				EditorUtility.SetDirty(warriorData);

				characterData = warriorData;

				break;
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		// rename the assets if the name was changed
		if (characterName != editedData.name) {
			Debug.Log(AssetDatabase.RenameAsset(dataPath, newDataPath));
			AssetDatabase.Refresh();

			Debug.Log(AssetDatabase.RenameAsset(prefabPath, newPrefabPath));
			AssetDatabase.Refresh();

			characterName = editedData.name;
		}
	}

	// Returns a clone of or clones a CharacterData object.
	private CharacterData cloneData(CharacterData sourceData, CharacterData destinationData = null) {
		switch (dataSetting) {
			case SettingsType.MAGE:
				if (destinationData == null)
					destinationData = CreateInstance<MageData>();

				((MageData)destinationData).damageType = ((MageData)sourceData).damageType;
				((MageData)destinationData).weaponType = ((MageData)sourceData).weaponType;

				break;
			case SettingsType.ROGUE:
				if (destinationData == null)
					destinationData = CreateInstance<RogueData>();

				((RogueData)destinationData).strategyType = ((RogueData)sourceData).strategyType;
				((RogueData)destinationData).weaponType = ((RogueData)sourceData).weaponType;

				break;
			case SettingsType.WARRIOR:
				if (destinationData == null)
					destinationData = CreateInstance<WarriorData>();

				((WarriorData)destinationData).classType = ((WarriorData)sourceData).classType;
				((WarriorData)destinationData).weaponType = ((WarriorData)sourceData).weaponType;

				break;
		}

		destinationData.prefab = sourceData.prefab;
		destinationData.name = sourceData.name;
		destinationData.maxHealth = sourceData.maxHealth;
		destinationData.maxEnergy = sourceData.maxEnergy;
		destinationData.power = sourceData.power;
		destinationData.critChance = sourceData.critChance;

		return destinationData;
	}
}
