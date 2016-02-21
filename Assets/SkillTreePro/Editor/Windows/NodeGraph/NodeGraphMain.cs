using UnityEngine;
using UnityEditor;
using System;

namespace Adnc.SkillTreePro {
	public class NodeGraphMain {
		const int padding = 10;
		Rect pos;
		GraphCamera camera = new GraphCamera();
		SkillCollectionDefinitionBase selectedNode;

		Event e;
		Vector2 mousePos;
		Vector2 mousePosGlobal;
		Vector2 nodeClickOffset;

		SkillCategoryDefinitionBase lastDef;

		bool MouseInBounds {
			get {
				bool horizontalValid = mousePos.x < Wm.Win.position.width - Wm.Win.sidebarWidth - GUI.skin.verticalScrollbar.fixedWidth;
				bool verticalValid = mousePos.y < Wm.Win.position.height - GUI.skin.horizontalScrollbar.fixedHeight;

				return horizontalValid && verticalValid;
			}
		}

		public void Update (Rect pos) {
			if (Wm.DbCat == null) return;

			this.pos = pos;

			if (lastDef != Wm.DbCat) {
				camera.Reset();
				lastDef = Wm.DbCat;
			}

			WrapperBegin();
			Content();
			WrapperEnd();
		}

		void Content () {
			Wm.Win.BeginWindows();
			if (Wm.DbCat != null) {
				DisplayNode(Wm.DbCat.start);
				Wm.DbCat.skillCollections.ForEach(sc => DisplayNode(sc));
			}
			Wm.Win.EndWindows();

			if (MouseInBounds) {
				MouseActive();
			}
		}

		void DisplayNode (SkillCollectionDefinitionBase col) {
			col.node.RectPos = GUI.Window(col.GetInstanceID(), col.node.RectPos, DrawNode, col.DisplayName);

			if (selectedNode == null && col.node.RectPos.Contains(mousePosGlobal) && e.type == EventType.mouseDown) {
				if (e.button == 0) {
					Wm.DbCol = col;
					selectedNode = col;
					selectedNode._drag = true;
					nodeClickOffset = selectedNode.node.RectPos.position - mousePosGlobal;

					if (selectedNode.Editable) {
						Selection.activeObject = selectedNode;
					} else {
						Selection.activeObject = null;
					}
				} else if (e.button == 1) {
					Wm.DbCol = col;
					selectedNode = col;
				}

				Wm.DbCol = col;
				selectedNode = col;
				nodeClickOffset = selectedNode.node.RectPos.position - mousePosGlobal;
			}
		}

		void MouseActive () {
			if (selectedNode != null) {
				if (e.button == 0 && selectedNode._drag) {
					selectedNode.node.RectPos = new Rect(mousePosGlobal.x + nodeClickOffset.x, mousePosGlobal.y + nodeClickOffset.y, 0, 0);
					Wm.Win.Repaint();
				} else if (e.button == 1 && e.type == EventType.mouseDown) {
					GenericMenu menu = new GenericMenu();

					if (selectedNode.Editable) {
						menu.AddItem(new GUIContent("Delete Skill Group"), false, DeleteSkillGroup);
					}

					menu.AddItem(new GUIContent("Add Child Transition"), false, null);

					menu.ShowAsContext();
					e.Use();
				}
			} else {
				if (e.button == 1 && e.type == EventType.mouseDown) {
					GenericMenu menu = new GenericMenu();
					Wm.Db.GetSkillCollectionTypes()
						.ForEach(t => menu.AddItem(new GUIContent(string.Format("Add Skill Group/{0}", t)), false, CreateCollection, t));
					menu.ShowAsContext();
					e.Use();
				} else if (e.button == 0) {
					if (e.type == EventType.mouseDown) {
						camera.BeginMove(mousePos);
					}
				}
			}
		}

		void DeleteSkillGroup () {
			if (!EditorUtility.DisplayDialog("Delete Skill Collection?", 
				"Are you sure you want to delete this skill collection? It will delete this collection plus all skills it contains.",
				"Delete Skill Collection", 
				"Cancel")) return;

				// @TODO Clean up 
//				SkillCollectionBase[] collect = target.currentCategory.GetComponentsInChildren<SkillCollectionBase>();
//				SkillCollectionBase t = collect[selectIndex];

				// Clean out all references to our skill collection
//				foreach (SkillCollectionBase node in collect) {
//					node.childSkills.Remove(t);
//				}

			Wm.DestroyCollection(Wm.DbCat, Wm.DbCol);
		}

		void WrapperBegin () {
			e = Event.current;
			DrawTitle();
			mousePos = e.mousePosition;
			camera.offset = GUI.BeginScrollView(pos, camera.offset, new Rect(camera.viewportSize / -2f, camera.viewportSize / -2f, camera.viewportSize, camera.viewportSize));
			mousePosGlobal = camera.GetMouseGlobal(mousePos);
		}

		void WrapperEnd () {
			GUI.EndScrollView();

			// Always stop the camera on mouse up (even if not in the window)
			if (Event.current.rawType == EventType.MouseUp) {
				if (selectedNode != null) {
					selectedNode._drag = false;
					selectedNode = null;
				}

				camera.EndMove();
			}

			// Poll and update the viewport if the camera has moved
			if (camera.PollCamera(mousePos)) {
				Wm.Win.Repaint();
			}
		}

		void CreateCollection (object obj) {
			string fullName = obj as string;
			string[] fullNameChunks = fullName.Split('.');
			string shortName = fullNameChunks[fullNameChunks.Length - 1];

			SkillCollectionDefinitionBase scsd = ScriptableObject.CreateInstance(shortName) as SkillCollectionDefinitionBase;
			scsd.Setup(Wm.DbCat);
			scsd.node.SetPosition(mousePosGlobal);
			AssetDatabase.AddObjectToAsset(scsd, Wm.Db);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		void DrawNode (int id) {
//			Event e = Event.current;

			// Check if this node was clicked
		}

		void DrawTitle () {
			string title = string.Format("{0}: {1}", Wm.Db.title, Wm.DbCat.DisplayName);
			GUI.Label(new Rect(10, 10, 100, 20), title, new GUIStyle {fontSize = 20});
		}
	}
}
