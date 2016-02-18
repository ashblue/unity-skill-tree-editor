using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.SkillTreePro {
	[CustomEditor(typeof(SkillTreeDatabase))]
	public class StatsDataDrawer : Editor {
		public override void OnInspectorGUI () {
			if (GUILayout.Button("Edit Skill Tree")) {
				Debug.Log("Call skill tree editor and inject asset path for loading");
			}

			serializedObject.ApplyModifiedProperties();
			DrawDefaultInspector();
			if (GUI.changed) EditorUtility.SetDirty(target);
		}
	}
}
