using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphController : EditorWindow {
		SkillTree target;
		GraphSidebar sidebar;
		GUIStyle textStyle;
		Vector2 mousePos;

		float sidebarWidth = 240f;

		void OnEnable () {
			titleContent.text = "Skill Tree";

			textStyle = new GUIStyle();
			textStyle.fontSize = 20;

			if (sidebar == null) {
				sidebar = new GraphSidebar();
			}

			UpdateTarget(Selection.activeGameObject);
		}

		[MenuItem("Window/Skill Tree")]
		static void ShowEditor () {
			EditorWindow.GetWindow<GraphController>();
		}

		void OnSelectionChange () {
			UpdateTarget(Selection.activeGameObject);
		}

		void UpdateTarget (GameObject go) {
			if (go != null) {
				SkillTree skillTree = go.GetComponent<SkillTree>();
				if (skillTree) {
					target = skillTree;
					sidebar.target = target;
				}
			}

			Repaint();
		}

		void OnGUI () {
			DrawTitle();

			Event e = Event.current;
			mousePos = e.mousePosition;

			// Context menu
			if (mousePos.x < position.width - sidebarWidth) {
				if (e.button == 1) {
					if (e.type == EventType.MouseDown) {
						GenericMenu menu = new GenericMenu();
						menu.AddItem(new GUIContent("Add Skill Group"), false, CreateSkillGroup);
						menu.ShowAsContext();
						e.Use();
					}
				}
			}

			BeginWindows();
			foreach (Transform child in target.currentCategory.transform) {
				SkillCollection node = child.gameObject.GetComponent<SkillCollection>();
				node.windowRect = GUI.Window(node.GetInstanceID(), node.windowRect, DrawNodeWindow, node.displayName);
			}
			EndWindows();

			sidebar.DrawSidebar(new Rect(position.width - sidebarWidth, 0, sidebarWidth, position.height), 10f, Color.gray);
		}

		void DrawNodeWindow (int id) {
			// @TODO Spit out the attached skills here (mostly copy / paste sidebar code)
			EditorGUILayout.TextField("test");
			GUI.DragWindow();
		}

		void CreateSkillGroup () {
			GameObject go = new GameObject();
			go.name = "SkillCollection";
			SkillCollection skill = go.AddComponent<SkillCollection>();
			skill.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
			go.transform.SetParent(target.currentCategory.transform);

			// @TODO Create the skill group at the current mouse position
		}

		void DrawTitle () {
			if (target != null) {
				string title = target.title;
				if (target.currentCategory != null) title += ": " + target.currentCategory.displayName;

				GUI.Label(new Rect(10, 10, 100, 20), title, textStyle);
			}
		}
	}
}
