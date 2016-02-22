using UnityEngine;
using UnityEditor;
using System;

namespace Adnc.SkillTreePro {
	public class NodeGraphMain {
		const int padding = 10;
		Rect pos;
		GraphCamera camera = new GraphCamera();
		GridPrinter grid = new GridPrinter();
		SkillCollectionDefinitionBase selectedNode;

		Event e;
		Vector2 mousePos;
		Vector2 mousePosGlobal;
		Vector2 nodeClickOffset;

		SkillCategoryDefinitionBase lastDef;
		bool isTransition;

		Vector2 WindowSize {
			get {
				return new Vector2(
					Wm.Win.position.width - Wm.Win.sidebarWidth - GUI.skin.verticalScrollbar.fixedWidth,
					Wm.Win.position.height - GUI.skin.horizontalScrollbar.fixedHeight
				);
			}
		}

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
				if (!isTransition) {
					MouseActive();
				} else {
					Transition();
				}
			}
		}

		SkillCollectionDefinitionBase GetCollectionByMouse () {
			for (int i = 0, l = Wm.DbCat.skillCollections.Count; i < l; i++) {
				SkillCollectionDefinitionBase col = Wm.DbCat.skillCollections[i];
				if (col.node.RectPos.Contains(mousePosGlobal)) {
					return col;
				}
			}

			return null;
		}

		/// <summary>
		/// Handles process of connecting parent to child nodes
		/// </summary>
		void Transition () {
			// Draw a transistion if in transistion mode
			if (isTransition && selectedNode != null) {
				Rect beginRect = selectedNode.node.RectPos;
				Rect mouseRect = new Rect(mousePosGlobal.x, mousePosGlobal.y, 10f, 10f);

				DrawNodeCurve(beginRect, mouseRect);

				Wm.Win.Repaint();
			}

			if (e.type == EventType.mouseDown) {
				SkillCollectionDefinitionBase col = GetCollectionByMouse();
				if (col == null || col == selectedNode || !col.AllowParents) {
					EndSkillGroupTransition();
					return;
				}

				if (e.button == 0) {
					selectedNode.childCollections.Remove(col);
					selectedNode.childCollections.Add(col);
					EndSkillGroupTransition();
				}
			}
		}

		void DisplayNode (SkillCollectionDefinitionBase col) {
			col.node.RectPos = GUI.Window(col.GetInstanceID(), col.node.RectPos, DrawNode, col.DisplayName);
			col.childCollections.ForEach(cc => DrawLineBottomToTop(col.node.RectPos, cc.node.RectPos));

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

					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Add Child Transition"), false, BeginSkillGroupTransition);
					selectedNode.childCollections
						.ForEach(c => menu.AddItem(
							new GUIContent(string.Format("Delete Child Transition/{0}", c.DisplayName)),
							false,
							DeleteSkillGroupTransition,
							c.uuid
						));


					menu.ShowAsContext();
					e.Use();
				}

				if (e.button == 0 && e.type == EventType.mouseUp) {
					selectedNode.node.RectPos = GridPrinter.SnapPosition(selectedNode.node.RectPos);
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

			// Offset the offset so it lines up in the middle
			grid.Update(WindowSize, new Vector2(camera.offset.x - (camera.viewportSize / 2), camera.offset.y - (camera.viewportSize / 2)));
		}

		void WrapperEnd () {
			GUI.EndScrollView();

			// Always stop the camera on mouse up (even if not in the window)
			if (Event.current.rawType == EventType.MouseUp && !isTransition) {
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
		}

		void DrawTitle () {
			string title = string.Format("{0}: {1}", Wm.Db.title, Wm.DbCat.DisplayName);
			GUI.Label(new Rect(10, 10, 100, 20), title, new GUIStyle {fontSize = 20});
		}

		void BeginSkillGroupTransition () {
			isTransition = true;
		}

		void DeleteSkillGroupTransition (object obj) {
			string uuid = obj as string;
			selectedNode.childCollections.RemoveAll(c => c.uuid == uuid);
			EditorUtility.SetDirty(Wm.Db);
		}

		void EndSkillGroupTransition () {
			EditorUtility.SetDirty(Wm.Db);
			isTransition = false;
			selectedNode = null;
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
	}
}
