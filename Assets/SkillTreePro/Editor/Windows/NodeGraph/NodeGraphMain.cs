using UnityEngine;
using UnityEditor;
using System;

namespace Adnc.SkillTreePro {
	public class NodeGraphMain {
		const int padding = 10;
		Rect pos;
		GraphCamera camera = new GraphCamera();
		SkillCollectionDefinition selectedNode;

		Vector2 mousePos;
		Vector2 mousePosGlobal;

		SkillCategoryDefinition lastDef;

		bool MouseInBounds {
			get {
				bool horizontalValid = mousePos.x < Wm.Win.position.width - Wm.Win.sidebarWidth - GUI.skin.verticalScrollbar.fixedWidth;
				bool verticalValid = mousePos.y < Wm.Win.position.height - GUI.skin.horizontalScrollbar.fixedHeight;

				return horizontalValid && verticalValid;
			}
		}

		public void Update (Rect pos) {
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
			Event e = Event.current;
			mousePosGlobal = camera.GetMouseGlobal(mousePos);

			Wm.Win.BeginWindows();
			if (Wm.DbCat != null) {
				SkillCollectionStartDefinition start = Wm.DbCat.start;
				start.node.RectPos = GUI.Window(-1, start.node.RectPos, DrawNode, start.displayName);

				if (selectedNode == null && e.button == 0 && e.type == EventType.mouseDown && start.node.RectPos.Contains(mousePosGlobal)) {
					selectedNode = Wm.DbCat.start;
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
					selectedNode.node.RectPos = new Rect(mousePosGlobal.x, mousePosGlobal.y, 0, 0);
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
			mousePos = Event.current.mousePosition;
			camera.offset = GUI.BeginScrollView(pos, camera.offset, new Rect(camera.viewportSize / -2f, camera.viewportSize / -2f, camera.viewportSize, camera.viewportSize));
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
			Event e = Event.current;

			// Check if this node was clicked
		}
	}
}
