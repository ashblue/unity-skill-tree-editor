using UnityEngine;
using UnityEditor;
using System;

namespace Adnc.SkillTreePro {
	public class NodeGraphMain {
		const int padding = 10;
		Rect pos;
		GraphCamera camera = new GraphCamera();
		SkillCollectionDefinition selectedNode;

		Event e;
		Vector2 mousePos;
		Vector2 mousePosGlobal;
		Vector2 nodeClickOffset;

		SkillCategoryDefinition lastDef;

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
				SkillCollectionStartDefinition start = Wm.DbCat.start;
				start.node.RectPos = GUI.Window(-1, start.node.RectPos, DrawNode, start.DisplayName);


				if (selectedNode == null && e.button == 0 && e.type == EventType.mouseDown && start.node.RectPos.Contains(mousePosGlobal)) {
					Wm.DbCol = Wm.DbCat.start;
					selectedNode = Wm.DbCat.start;
					nodeClickOffset = selectedNode.node.RectPos.position - mousePosGlobal;
				}
			}
			Wm.Win.EndWindows();

			if (MouseInBounds) {
				MouseActive();
			}
		}

		void MouseActive () {
			Event e = Event.current;

			if (selectedNode != null) {
				if (e.button == 0) {
					selectedNode.node.RectPos = new Rect(mousePosGlobal.x + nodeClickOffset.x, mousePosGlobal.y + nodeClickOffset.y, 0, 0);
					Wm.Win.Repaint();
				}
			} else {
				if (e.button == 1) {

				} else if (e.button == 0) {
					if (e.type == EventType.mouseDown) {
						camera.BeginMove(mousePos);
					}
				}
			}
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
				selectedNode = null;
				camera.EndMove();
			}

			// Poll and update the viewport if the camera has moved
			if (camera.PollCamera(mousePos)) {
				Wm.Win.Repaint();
			}
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
