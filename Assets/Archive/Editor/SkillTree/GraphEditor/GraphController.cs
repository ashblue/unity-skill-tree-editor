using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphController : EditorWindow {
		public static GraphController current;
		SkillTreeBase target;
		GraphSidebar sidebar;
		public GraphCamera camera;

		GUIStyle textStyle; // Style used for title in upper left
		Vector2 mousePos; // Local screen mouse position
		Vector2 mousePosGlobal; // Global mouse position based on camera offsets
		bool isTransition; // Are we in transistion mode
		SkillCollectionBase[] collect = new SkillCollectionBase[0];

		float sidebarWidth = 240f; // Size of the sidebar
		int selectIndex = -1; // Currently selected window
		SkillCollectionBase selectNode; // Actively selected node for a transition

		SkillCollectionBase snapNode; // We need to refresh this node and force snap it
		public bool forceSnapping = true;

		void OnEnable () {
			titleContent.text = "Skill Tree";

			textStyle = new GUIStyle();
			textStyle.fontSize = 20;

			if (sidebar == null) sidebar = new GraphSidebar();

			if (camera == null) camera = new GraphCamera();

			current = this;
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
				SkillTreeBase skillTree = go.GetComponent<SkillTreeBase>();
				if (skillTree) {
					// Verify inheritance on SkillTree
					if (IsValidInheritance(skillTree)) {
						// Assign the new skill tree
						target = skillTree;
						sidebar.target = target;
						camera.Reset();
					} else {
						Debug.LogError("Invalid inheritance for skill classes. Please verify they inherit from proper base classes before proceeding.");
					}
				}
			}

			Repaint();
		}

		// Checks if the current skill tree classes properly inherit from a base class
		bool IsValidInheritance (SkillTreeBase target) {
			if (!target.SkillCategory.IsSubclassOf(typeof(SkillCategoryBase))) {
				return false;
			} else if (!target.SkillCollection.IsSubclassOf(typeof(SkillCollectionBase))) {
				return false;
			} else if (!target.Skill.IsSubclassOf(typeof(SkillBase))) {
				return false;
			}

			return true;
		}

		void OnGUI () {
			if (target == null) return;

			DrawTitle();

			Event e = Event.current;
			mousePos = e.mousePosition; // Mouse position local to the viewing window
			mousePosGlobal = camera.GetMouseGlobal(e.mousePosition); // Mouse position local to the scroll window
			bool clickedNode = false;

			// Poll the current skill collection
			if (target.currentCategory != null) {
				collect = target.currentCategory.GetComponentsInChildren<SkillCollectionBase>();
			} else {
				collect = new SkillCollectionBase[0];
			}

			// Main content area
			if (target.currentCategory != null) {
				// Clicked outside of sidebar and scrollbar GUI
				if (mousePos.x < position.width - sidebarWidth - GUI.skin.verticalScrollbar.fixedWidth &&
				    mousePos.y < position.height - GUI.skin.horizontalScrollbar.fixedHeight) {

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
								menu.AddItem(new GUIContent("Add Skill"), false, AddSkill, collect[selectIndex]);
								menu.AddItem(new GUIContent("Add Child Transition"), false, BeginSkillGroupTransition);
								menu.AddSeparator("");
								menu.AddItem(new GUIContent("Delete Skill Group"), false, DeleteSkillGroup);
								menu.AddSeparator("");

								foreach (SkillCollectionBase child in collect[selectIndex].childSkills) {
									menu.AddItem(new GUIContent("Delete Transition " + child.displayName), false, DeleteSkillGroupTransition, new TransitionParentChild {
										parent = collect[selectIndex],
										child = child
									});
								}

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
									snapNode = collect[i];
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
						} else if (e.type == EventType.MouseUp && snapNode != null) {
							if (forceSnapping) SnapNodeToGrid(snapNode);
							snapNode = null;
						}
					}
				}

				camera.offset = GUI.BeginScrollView(new Rect(0f, 0f, position.width - sidebarWidth, position.height), camera.offset, new Rect(camera.viewportSize / -2f, camera.viewportSize / -2f, camera.viewportSize, camera.viewportSize));
				
				BeginWindows();
				for (int i = 0; i < collect.Length; i++) {
					collect[i].windowRect = GUI.Window(i, collect[i].windowRect, DrawNodeWindow, collect[i].displayName);
					
					foreach (SkillCollectionBase child in collect[i].childSkills) {
						DrawLineBottomToTop(collect[i].windowRect, child.windowRect);
					}
				}
				EndWindows();
				
				GUI.EndScrollView(); // Camera scroll for windows
			}

			// Draw a transistion if in transistion mode
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

		public void SnapNodeToGrid (SkillCollectionBase node) {
			Vector2 snapRatio = target.gridCellSize;
			node.windowRect.x = Mathf.Round(node.windowRect.x / snapRatio.x) * snapRatio.x;
			node.windowRect.y = Mathf.Round(node.windowRect.y / snapRatio.y) * snapRatio.y;
			// Get the currently active SkillTreeBase and 
		}

		public void SnapAllNodesToGrid () {
			foreach (SkillCollectionBase col in collect) {
				SnapNodeToGrid(col);
			}
		}

		Vector2 scrollPos;

		void BeginSkillGroupTransition () {
			isTransition = true;
			SkillCollectionBase[] collect = target.currentCategory.GetComponentsInChildren<SkillCollectionBase>();
			selectNode = collect[selectIndex];
		}

		void DeleteSkillGroupTransition (object obj) {
			TransitionParentChild parentChild = obj as TransitionParentChild;
			parentChild.parent.childSkills.Remove(parentChild.child);
		}

		void EndSkillGroupTransition () {
			isTransition = false;
			selectNode = null;
		}

		public static void DrawLineBottomToTop (Rect start, Rect end) {
			Vector3 startPos = new Vector3(start.x + (start.width / 2f), start.y + start.height, 0f);
			Vector3 endPos = new Vector3(end.x + (end.width / 2f), end.y, 0f);
			Vector3 startTan = startPos + Vector3.up * 50f;
			Vector3 endTan = endPos - Vector3.up * 50f;
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
			SkillBase deleteSkill = null;
			foreach (SkillBase skill in collect[id].GetComponentsInChildren<SkillBase>()) {
				GUILayout.BeginHorizontal();
				skill.unlocked = GUILayout.Toggle(skill.unlocked, "");

				if (GUILayout.Button(skill.displayName, GUILayout.Width(100f))) {
					Selection.activeGameObject = skill.gameObject;
				}

				if (GUILayout.Button("UP")) {
					skill.transform.SetSiblingIndex(skill.transform.GetSiblingIndex() - 1);
				}

				if (GUILayout.Button("DN")) {
					skill.transform.SetSiblingIndex(skill.transform.GetSiblingIndex() + 1);
				}

				if (GUILayout.Button("X")) {
					if (EditorUtility.DisplayDialog("Delete Skill?", 
					                                "Are you sure you want to delete this skill? This cannot be undone.",
					                                "Delete Skill", 
					                                "Cancel")) {
						deleteSkill = skill;
					}
				}

				GUILayout.EndHorizontal();
			}

			if (deleteSkill != null) DestroyImmediate(deleteSkill.gameObject);

			GUI.DragWindow();
		}

		void CreateSkillGroup () {
			GameObject go = new GameObject();
			go.name = "SkillCollection";
			SkillCollectionBase skill = go.AddComponent(target.SkillCollection) as SkillCollectionBase;
			skill.windowRect = new Rect(mousePosGlobal.x, mousePosGlobal.y, 220, 150);
			go.transform.SetParent(target.currentCategory.transform);

			AddSkill(skill);
		}

		void DeleteSkillGroup () {
			if (EditorUtility.DisplayDialog("Delete Skill Collection?", 
			                                "Are you sure you want to delete this skill collection? It will delete this collection plus all skills it contains.",
			                                "Delete Skill Collection", 
			                                "Cancel")) {
				SkillCollectionBase[] collect = target.currentCategory.GetComponentsInChildren<SkillCollectionBase>();
				SkillCollectionBase t = collect[selectIndex];
				
				// Clean out all references to our skill collection
				foreach (SkillCollectionBase node in collect) {
					node.childSkills.Remove(t);
				}
				
				DestroyImmediate(t.gameObject);
			}
		}

		void AddSkill (object obj) {
			SkillCollectionBase col = obj as SkillCollectionBase;
			AddSkill(col);
		}

		void AddSkill (SkillCollectionBase col) {
			GameObject go = new GameObject();
			go.name = "Skill";
			SkillBase s = go.AddComponent(target.Skill) as SkillBase;
			s.Uuid = Guid.NewGuid().ToString();
			go.transform.SetParent(col.transform);
		}

		void DrawTitle () {
			if (target != null) {
				string title = target.displayName;
				if (target.currentCategory != null) title += ": " + target.currentCategory.displayName;

				GUI.Label(new Rect(10, 10, 100, 20), title, textStyle);
			}
		}
	}
}
