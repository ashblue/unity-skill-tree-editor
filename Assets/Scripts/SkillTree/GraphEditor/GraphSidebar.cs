using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphSidebar {
		public SkillTree target;

		public void DrawSidebar (Rect rect, float padding, Color color) {
			float innerWidth = rect.width - (padding * 2f);
			float innerHeight = rect.height - (padding * 2f);

			GUI.BeginGroup(rect); // Container
			
			DrawBox(new Rect(0, 0, rect.width, rect.height), color);

			GUI.BeginGroup(new Rect(padding, padding, innerWidth, innerHeight)); // Padding

			if (target != null) {
				float y = 0f;
				foreach (Transform child in target.transform) {
					GUI.BeginGroup(new Rect(0f, y, innerWidth, 300f));
					
					if (GUI.Button(new Rect(0f, 0f, innerWidth - 25f, 20f), child.name)) {
						target.currentCategory = child.GetComponent<SkillCategory>();
						Selection.activeGameObject = child.gameObject;
					}
					
					if (GUI.Button(new Rect(innerWidth - 20f, 0f, 20f, 20f), "X")) {
						if (EditorUtility.DisplayDialog("Delete Category?", 
						                                "Are you sure you want to delete this category? The delete action cannot be undone.",
						                                "Delete Category", 
						                                "Cancel")) {

							if (target.currentCategory == child.GetComponent<SkillCategory>())
								target.currentCategory = null;

							GameObject.DestroyImmediate(child.gameObject);
						}
					}
					
					GUI.EndGroup();
					y += 24f;
				}
			}
			
			GUI.EndGroup(); // Padding
			GUI.EndGroup(); // Container
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
