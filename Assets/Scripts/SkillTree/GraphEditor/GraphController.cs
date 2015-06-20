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
		int selectIndex = -1; // Currently selected window

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
			bool clickedNode = false;
			SkillCollection[] collect = target.currentCategory.GetComponentsInChildren<SkillCollection>();


			// Context menu
			if (mousePos.x < position.width - sidebarWidth) {

				// Context menu
				if (e.button == 1) {
					if (e.type == EventType.MouseDown) {
						for (int i = 0; i < collect.Length; i++) {
							if (collect[i].windowRect.Contains(mousePos)) {
								selectIndex = i;
								clickedNode = true;
								break;
							}
						}

						if (clickedNode) {
							GenericMenu menu = new GenericMenu();
							menu.AddItem(new GUIContent("Delete Skill Group"), false, DeleteSkillGroup);
							menu.ShowAsContext();
							e.Use();
						} else {
							GenericMenu menu = new GenericMenu();
							menu.AddItem(new GUIContent("Add Skill Group"), false, CreateSkillGroup);
							menu.ShowAsContext();
							e.Use();
						}
					}
				} else if (e.button == 0) {
					if (e.type == EventType.MouseDown) {
						for (int i = 0; i < collect.Length; i++) {
							if (collect[i].windowRect.Contains(mousePos)) {
								selectIndex = i;
								clickedNode = true;
								break;
							}
						}

						if (clickedNode) {
							Selection.activeGameObject = target.currentCategory.transform.GetChild(selectIndex).gameObject;
						} else {
							// @TODO Integrate camera drag here
						}
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
		}

		void DeleteSkillGroup () {
			Transform t = target.currentCategory.transform.GetChild(selectIndex);
			DestroyImmediate(t.gameObject);
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
