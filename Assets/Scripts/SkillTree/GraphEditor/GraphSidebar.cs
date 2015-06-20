using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphSidebar {
		public SkillTree target;
		Texture2D texture;

		public void DrawSidebar (Rect rect, float padding, Color color) {
			float innerWidth = rect.width - (padding * 2f);
			float innerHeight = rect.height - (padding * 2f);

			GUI.BeginGroup(rect); // Container
			
			DrawBox(new Rect(0, 0, rect.width, rect.height), color);

			GUI.BeginGroup(new Rect(padding, padding, innerWidth, innerHeight)); // Padding

			if (target != null) {
				float y = 0f;
				foreach (Transform child in target.transform) {
					SkillCategory cat = child.GetComponent<SkillCategory>();
					GUI.BeginGroup(new Rect(0f, y, innerWidth, 300f));

					if (GUI.Button(new Rect(0f, 0f, 22f, 20f), "X")) {
						if (EditorUtility.DisplayDialog("Delete Category?", 
						                                "Are you sure you want to delete this category? The delete action cannot be undone.",
						                                "Delete Category", 
						                                "Cancel")) {
							if (target.currentCategory == cat)
								target.currentCategory = null;
							
							GameObject.DestroyImmediate(cat.gameObject);
						}
					}

					if (GUI.Button(new Rect(24f, 0f, innerWidth - 82f, 20f), cat.displayName)) {
						target.currentCategory = cat;
						Selection.activeGameObject = cat.gameObject;
					}

					if (GUI.Button(new Rect(innerWidth - 56f, 0f, 27f, 20f), "UP")) {
						child.SetSiblingIndex(child.GetSiblingIndex() - 1);
					}
					
					if (GUI.Button(new Rect(innerWidth - 27f, 0f, 27f, 20f), "DN")) {
						child.SetSiblingIndex(child.GetSiblingIndex() + 1);
					}
					
					GUI.EndGroup();
					y += 24f;
				}

				if (GUI.Button(new Rect(0f, y, innerWidth, 20f), "Create Category")) {
					GameObject go = new GameObject();
					go.name = "Category";
					go.AddComponent<SkillCategory>();
					go.transform.SetParent(target.transform);
				}
			}
			
			GUI.EndGroup(); // Padding
			GUI.EndGroup(); // Container
		}
		
		void DrawBox (Rect position, Color color) {
			if (texture == null) texture = new Texture2D(1, 1);
			texture.SetPixel(0,0,color);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(position, GUIContent.none);
		}
	}
}
