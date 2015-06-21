using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphController : EditorWindow {
		SkillTree target;
		GraphSidebar sidebar;
		GraphCamera camera;

		GUIStyle textStyle; // Style used for title in upper left
		Vector2 mousePos; // Local screen mouse position
		Vector2 mousePosGlobal; // Global mouse position based on camera offsets
		bool isTransition; // Are we in transistion mode

		float sidebarWidth = 240f; // Size of the sidebar
		int selectIndex = -1; // Currently selected window
		SkillCollection selectNode; // Actively selected node for a transition

		void OnEnable () {
			titleContent.text = "Skill Tree";

			textStyle = new GUIStyle();
			textStyle.fontSize = 20;

			if (sidebar == null) sidebar = new GraphSidebar();

			if (camera == null) camera = new GraphCamera();
			camera.Reset();

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
					camera.Reset();
				}
			}

			Repaint();
		}

		void OnGUI () {
			if (target == null) return;

			DrawTitle();

			Event e = Event.current;
			mousePos = e.mousePosition; // Mouse position local to the viewing window
			mousePosGlobal = camera.GetMouseGlobal(e.mousePosition); // Mouse position local to the scroll window
			bool clickedNode = false;

			if (target.currentCategory != null) {

				SkillCollection[] collect = target.currentCategory.GetComponentsInChildren<SkillCollection>();

				// Clicked outside sidebar
				if (mousePos.x < position.width - sidebarWidth) {

					// Context menu
					if (e.button == 1 && !isTransition) {
						if (e.type == EventType.MouseDown) {
							for (int i = 0; i < collect.Length; i++) {
								if (collect[i].windowRect.Contains(mousePosGlobal)) {
									selectIndex = i;
									clickedNode = true;
									break;
								}
							}

							if (clickedNode) {
								GenericMenu menu = new GenericMenu();
								menu.AddItem(new GUIContent("Add Child Transition"), false, BeginSkillGroupTransition);
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
								if (collect[i].windowRect.Contains(mousePosGlobal)) {
									selectIndex = i;
									clickedNode = true;
									break;
								}
							}

							if (isTransition) {
								selectNode.childSkills.Remove(collect[selectIndex]);
								selectNode.childSkills.Add(collect[selectIndex]);
								EndSkillGroupTransition();
							} else {
								if (clickedNode) {
									Selection.activeGameObject = target.currentCategory.transform.GetChild(selectIndex).gameObject;
								} else {
									camera.BeginMove(mousePos);
								}
							}
						}
					}
				}

				camera.offset = GUI.BeginScrollView(new Rect(0f, 0f, position.width - sidebarWidth, position.height), camera.offset, new Rect(camera.viewportSize / -2f, camera.viewportSize / -2f, camera.viewportSize, camera.viewportSize));
				
				BeginWindows();
				foreach (SkillCollection node in collect) {
					node.windowRect = GUI.Window(node.GetInstanceID(), node.windowRect, DrawNodeWindow, node.displayName);

					foreach (SkillCollection child in node.childSkills) {
						DrawLineBottomToTop(node.windowRect, child.windowRect);
					}
				}
				EndWindows();
				
				GUI.EndScrollView(); // Camera scroll for windows
			}

			// Draw the transistion
			if (isTransition && selectNode != null) {
				Vector2 globalOffset = camera.GetOffsetGlobal();
				Rect beginRect = selectNode.windowRect;
				beginRect.x -= globalOffset.x;
				beginRect.y -= globalOffset.y;
				Rect mouseRect = new Rect(mousePos.x, mousePos.y, 10f, 10f);

				DrawNodeCurve(beginRect, mouseRect);

				Repaint();
			}

			sidebar.DrawSidebar(new Rect(position.width - sidebarWidth, 0, sidebarWidth, position.height), 10f, Color.gray);

			// Always stop the camera on mouse up (even if not in the window)
			if (Event.current.rawType == EventType.MouseUp) {
				camera.EndMove();
			}

			// Poll and update the viewport if the camera has moved
			if (camera.PollCamera(mousePos)) {
				Repaint();
			}
		}

		Vector2 scrollPos;

		void BeginSkillGroupTransition () {
			isTransition = true;
			SkillCollection[] collect = target.currentCategory.GetComponentsInChildren<SkillCollection>();
			selectNode = collect[selectIndex];
		}

		void EndSkillGroupTransition () {
			isTransition = false;
			selectNode = null;
		}

		public static void DrawLineBottomToTop (Rect start, Rect end) {
			Vector3 startPos = new Vector3(start.x + (start.width / 2f), start.y + start.height, 0f);
			Vector3 endPos = new Vector3(end.x + (end.width / 2f), end.y, 0f);
			Vector3 startTan = startPos + Vector3.up * 50f;
			Vector3 endTan = endPos + Vector3.up * 50f;
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 3f);
		}

		public static void DrawNodeCurve (Rect start, Rect end) {
			Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
			Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
			Vector3 startTan = startPos + Vector3.down * 50;
			Vector3 endTan = endPos + Vector3.up * 50;
			Color shadowCol = new Color(0, 0, 0, 0.06f);
			
			for (int i = 0; i < 3; i++) {
				Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
			}
			
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
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
			skill.windowRect = new Rect(mousePosGlobal.x, mousePosGlobal.y, 200, 150);
			go.transform.SetParent(target.currentCategory.transform);
		}

		void DeleteSkillGroup () {
			SkillCollection[] collect = target.currentCategory.GetComponentsInChildren<SkillCollection>();
			SkillCollection t = collect[selectIndex];

			// Clean out all references to our skill collection
			foreach (SkillCollection node in collect) {
				node.childSkills.Remove(t);
			}

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
