using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Adnc.SkillTree {
	[CustomEditor(typeof(SkillTreeBase), true)]
	public class SkillTreeEditor : Editor {
		EditorWindow window;

		public override void OnInspectorGUI () {
			DrawDefaultInspector();

			if (GUILayout.Button("Edit Skill Tree")) {
				window = EditorWindow.GetWindow<GraphController>();
				window.Show();
			}
			
			// If this isn't set your changes will not take effect
			if (GUI.changed) EditorUtility.SetDirty(target);
		}
	}
}
