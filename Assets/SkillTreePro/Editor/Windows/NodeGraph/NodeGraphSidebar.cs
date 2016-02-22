using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Adnc.SkillTreePro {
	public enum NodeGraphSidebarTab {
		Categories,
		Skills
	}

	public class NodeGraphSidebar {
		Rect pos;
		Color backgroundColor = Color.white;
		const int padding = 10;
		NodeGraphSidebarTab tab = NodeGraphSidebarTab.Categories;

		public void Update (Rect pos) {
			this.pos = pos;

			WrapperBegin();
			Content();
			WrapperEnd();
		}

		void Content () {
			EditorGUI.BeginChangeCheck();

			if (tab == NodeGraphSidebarTab.Categories) {
				Categories();
			} else if (tab == NodeGraphSidebarTab.Skills) {
				Skills();
			}

			if (EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(Wm.Db);
			}
		}

		void Skills () {
			EditorGUILayout.LabelField("Skills", EditorStyles.boldLabel);

			int deleteIndex = -1;
			int upIndex = -1;
			int downIndex = -1;

			if (Wm.DbCol == null || !Wm.DbCol.Editable) {
				EditorGUILayout.LabelField("Please select a valid skill collection to edit skill entries", EditorStyles.helpBox);
				return;
			}

			Color defaultBackgroundColor = GUI.backgroundColor;
			for (int i = 0, l = Wm.DbCol.skills.Count; i < l; i++) {
				SkillDefinitionBase s = Wm.DbCol.skills[i];

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				EditorGUILayout.LabelField(s.DisplayName);

				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("Edit")) {
					Selection.activeObject = s;
				}

				if (Wm.DbCol.skillIndex == i) GUI.backgroundColor = Color.green;
				if (GUILayout.Button("Active")) {
					Wm.DbCol.skillIndex = i;
					Selection.activeObject = s;
				}
				GUI.backgroundColor = defaultBackgroundColor;

				if (GUILayout.Button("Destroy")) {
					deleteIndex = i;
				}

				if (GUILayout.Button("UP")) {
					upIndex = i;
				}

				if (GUILayout.Button("DN")) {
					downIndex = i;
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}

			if (deleteIndex > -1) {
				Wm.DestroySkill(Wm.DbCat, Wm.DbCol, Wm.DbCol.skills[deleteIndex]);
			} else if (upIndex > -1) {
				MoveSkill(Wm.DbCol.skills[upIndex], true);
			} else if (downIndex > -1) {
				MoveSkill(Wm.DbCol.skills[downIndex], false);
			}

			if (GUILayout.Button("Add Skill Entry")) {
				ShowSkillEntryMenu();
			}
		}

		void Inspector () {
			if (Wm.DbCol == null) {
				EditorGUILayout.LabelField("Please select a skill collection to edit.");
				return;
			}
		}

		void Categories () {
			EditorGUILayout.LabelField("Categories", EditorStyles.boldLabel);

			int deleteIndex = -1;
			int upIndex = -1;
			int downIndex = -1;

			for (int i = 0, l = Wm.Db.categories.Count; i < l; i++) {
				SkillCategoryDefinitionBase cat = Wm.Db.categories[i];
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				if (Wm.DbCat == cat) {
					cat._displayName = EditorGUILayout.TextField(cat._displayName);
				} else {
					EditorGUILayout.LabelField(cat.DisplayName);
				}

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Edit", GUILayout.ExpandWidth(false))) {
					Wm.DbCat = cat;
					Selection.activeObject = cat;
				}

				if (Wm.DbCat != cat && GUILayout.Button("Destroy", GUILayout.ExpandWidth(false))) {
					deleteIndex = i;
				}

				if (GUILayout.Button("Up", GUILayout.ExpandWidth(false))) {
					upIndex = i;
				}

				if (GUILayout.Button("Down", GUILayout.ExpandWidth(false))) {
					downIndex = i;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}

			if (deleteIndex > -1) {
				Wm.DestroyCategory(deleteIndex);
			} else if (upIndex > -1) {
				MoveCategory(upIndex, true);
			} else if (downIndex > -1) {
				MoveCategory(downIndex, false);
			}

			if (GUILayout.Button("Add Category")) {
				ShowCategoryMenu();
			}
		}

		void ShowSkillEntryMenu () {
			GenericMenu menu = new GenericMenu();
			Wm.Db.GetSkillTypes()
				.ForEach(t => menu.AddItem(new GUIContent(string.Format("Add Skill Entry/{0}", t)), false, CreateSkillEntry, t));
			menu.ShowAsContext();
			Event.current.Use();
		}

		void ShowCategoryMenu () {
			GenericMenu menu = new GenericMenu();
			Wm.Db.GetSkillCategoryTypes()
				.ForEach(t => menu.AddItem(new GUIContent(string.Format("Add Category/{0}", t)), false, CreateCategory, t));
			menu.ShowAsContext();
			Event.current.Use();
		}

		void CreateCategory (object obj) {
			string fullName = obj as string;
			string[] fullNameChunks = fullName.Split('.');
			string shortName = fullNameChunks[fullNameChunks.Length - 1];

			SkillCategoryDefinitionBase scd = ScriptableObject.CreateInstance(shortName) as SkillCategoryDefinitionBase;
			scd.Setup();
			AssetDatabase.AddObjectToAsset(scd, Wm.Db);

			SkillCollectionStartDefinition scsd = ScriptableObject.CreateInstance("SkillCollectionStartDefinition") as SkillCollectionStartDefinition;
			scsd._displayName = "Start";
			scsd.Setup(Wm.DbCat);
			AssetDatabase.AddObjectToAsset(scsd, Wm.Db);

			scd.start = scsd;
			Wm.Db.categories.Add(scd);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		void CreateSkillEntry (object obj) {
			string fullName = obj as string;
			string[] fullNameChunks = fullName.Split('.');
			string shortName = fullNameChunks[fullNameChunks.Length - 1];

			SkillDefinitionBase sd = ScriptableObject.CreateInstance(shortName) as SkillDefinitionBase;
			sd.Setup(Wm.DbCat, Wm.DbCol);
			AssetDatabase.AddObjectToAsset(sd, Wm.Db);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		void MoveSkill (SkillDefinitionBase skill, bool up) {
			int index = Wm.DbCol.skills.IndexOf(skill);
			int max = Wm.DbCol.skills.Count - 1;

			Wm.DbCol.skills.Remove(skill);

			if (up) {
				Wm.DbCol.skills.Insert(Mathf.Max(index - 1, 0), skill);
			} else {
				Wm.DbCol.skills.Insert(Mathf.Min(index + 1, max), skill);
			}

			EditorUtility.SetDirty(Wm.Db);
		}

		void MoveCategory (int index, bool up) {
			SkillCategoryDefinitionBase cat = Wm.Db.categories[index];
			int max = Wm.Db.categories.Count - 1;

			Wm.Db.categories.RemoveAt(index);

			if (up) {
				Wm.Db.categories.Insert(Mathf.Max(index - 1, 0), cat);
			} else {
				Wm.Db.categories.Insert(Mathf.Min(index + 1, max), cat);
			}

			EditorUtility.SetDirty(Wm.Db);
		}

		void WrapperBegin () {
			float innerWidth = pos.width - (padding * 2f);
			float innerHeight = pos.height - (padding * 2f);

			GUILayout.BeginArea(pos); // Container
			DrawBox(new Rect(0, 0, pos.width, pos.height), backgroundColor);
			GUILayout.BeginArea(new Rect(padding, padding, innerWidth, innerHeight)); // Padding

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Categories")) tab = NodeGraphSidebarTab.Categories;
			if (GUILayout.Button("Skills")) tab = NodeGraphSidebarTab.Skills;
			EditorGUILayout.EndHorizontal();
		}

		void WrapperEnd () {
			GUILayout.EndArea();
			GUILayout.EndArea();
		}

		static void DrawBox (Rect position, Color color) {
			Color oldColor = GUI.color;

			GUI.color = color;
			GUI.Box(position, "");

			GUI.color = oldColor;
		}
	}
}
