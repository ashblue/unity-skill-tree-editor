using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphController : EditorWindow {
		// Lock the currently active target
		// @TODO Not implemented
		bool lockTarget;
		SkillTree target;

		GUIStyle textStyle;

		GraphSidebar sidebar;

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
			if (go != null && !lockTarget) {
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
			sidebar.DrawSidebar(new Rect(position.width - 200, 0, 200, position.height), 10f, Color.gray);
		}

		void DrawTitle () {
			if (target != null) {
				string title = target.title;
				if (target.currentCategory != null) title += ": " + target.currentCategory.name;

				GUI.Label(new Rect(10, 10, 100, 20), title, textStyle);
			}
		}
	}
}
