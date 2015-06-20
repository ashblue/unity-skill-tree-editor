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

		void OnEnable () {
			titleContent.text = "Skill Tree";
			UpdateTarget(Selection.activeGameObject);

			textStyle = new GUIStyle();
			textStyle.fontSize = 20;
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
				}
			}

			Repaint();
		}

		void OnGUI () {
			DrawSidebar(200, Color.gray);
			DrawTitle();
		}

		void DrawTitle () {
			if (target != null) {
				GUI.Label(new Rect(10, 10, 100, 20), target.title, textStyle);
			}
		}

		void DrawSidebar (int width, Color color) {
			DrawBox(new Rect(position.width - width, 0, width, position.height), color);
		}

		void DrawBox (Rect position, Color color) {
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0,0,color);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(position, GUIContent.none);
		}
	}
}
