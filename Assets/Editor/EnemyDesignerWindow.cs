using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Types;

public class EnemyDesignerWindow : EditorWindow {
	private Texture2D headerSectionTexture;
	private Texture2D mageSectionTexture;
	private Texture2D rogueSectionTexture;
	private Texture2D warriorSectionTexture;

	private Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

	private Rect headerSection;
	private Rect mageSection;
	private Rect rogueSection;
	private Rect warriorSection;

	private static MageData mageData;
	private static RogueData rogueData;
	private static WarriorData warriorData;

	public static MageData MageInfo { get { return mageData; } }
	public static RogueData RogueInfo { get { return rogueData; } }
	public static WarriorData WarriorInfo { get { return warriorData; } }

	[MenuItem("Window/Enemy Designer")]
	private static void OpenWindow() {
		EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
		window.minSize = new Vector2(600, 300);
		window.Show();
	}

	private void OnEnable() {
		InitTextures();
		InitData();
	}

	public static void InitData() {
		mageData = (MageData)CreateInstance(typeof(MageData));
		rogueData = (RogueData)CreateInstance(typeof(RogueData));
		warriorData = (WarriorData)CreateInstance(typeof(WarriorData));
	}

	private void InitTextures() {
		headerSectionTexture = new Texture2D(1, 1);
		headerSectionTexture.SetPixel(0, 0, headerSectionColor);
		headerSectionTexture.Apply();

		mageSectionTexture = Resources.Load<Texture2D>("icons/editorMageGradient");
		rogueSectionTexture = Resources.Load<Texture2D>("icons/editorRogueGradient");
		warriorSectionTexture = Resources.Load<Texture2D>("icons/editorWarriorGradient");
	}

	private void OnGUI() {
		DrawLayouts();
		DrawHeader();
		DrawMageSettings();
		DrawRogueSettings();
		DrawWarriorSettings();
	}

	private void DrawLayouts() {
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50;

		mageSection.x = 0;
		mageSection.y = 50;
		mageSection.width = Screen.width / 3f;
		mageSection.height = Screen.height - 50;

		rogueSection.x = Screen.width / 3f;
		rogueSection.y = 50;
		rogueSection.width = Screen.width / 3f;
		rogueSection.height = Screen.height - 50;

		warriorSection.x = (Screen.width / 3f) * 2;
		warriorSection.y = 50;
		warriorSection.width = Screen.width / 3f;
		warriorSection.height = Screen.height - 50;

		GUI.DrawTexture(headerSection, headerSectionTexture);
		GUI.DrawTexture(mageSection, mageSectionTexture);
		GUI.DrawTexture(rogueSection, rogueSectionTexture);
		GUI.DrawTexture(warriorSection, warriorSectionTexture);
	}

	private void DrawHeader() {
		GUILayout.BeginArea(headerSection);

		GUILayout.Label("Enemy Designer");

		GUILayout.EndArea();
	}

	private void DrawMageSettings() {
		GUILayout.BeginArea(mageSection);

		GUILayout.Label("Mage");

		// damage type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Damage:");
		mageData.damageType = (MageDamageType)EditorGUILayout.EnumPopup(mageData.damageType);
		EditorGUILayout.EndHorizontal();

		// weapon type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon:");
		mageData.weaponType = (MageWeaponType)EditorGUILayout.EnumPopup(mageData.weaponType);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Create!", GUILayout.Height(40))) {
			GeneralSettings.OpenWindow(GeneralSettings.SettingsType.MAGE);
		}

		GUILayout.EndArea();
	}

	private void DrawRogueSettings() {
		GUILayout.BeginArea(rogueSection);

		GUILayout.Label("Rogue");

		// strategy type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Darogue:");
		rogueData.strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(rogueData.strategyType);
		EditorGUILayout.EndHorizontal();

		// weapon type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon:");
		rogueData.weaponType = (RogueWeaponType)EditorGUILayout.EnumPopup(rogueData.weaponType);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Create!", GUILayout.Height(40))) {
			GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ROGUE);
		}

		GUILayout.EndArea();
	}

	private void DrawWarriorSettings() {
		GUILayout.BeginArea(warriorSection);

		GUILayout.Label("Warrior");

		// class type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Class:");
		warriorData.classType = (WarriorClassType)EditorGUILayout.EnumPopup(warriorData.classType);
		EditorGUILayout.EndHorizontal();

		// weapon type
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon:");
		warriorData.weaponType = (WarriorWeaponType)EditorGUILayout.EnumPopup(warriorData.weaponType);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Create!", GUILayout.Height(40))) {
			GeneralSettings.OpenWindow(GeneralSettings.SettingsType.WARRIOR);
		}

		GUILayout.EndArea();
	}
}
