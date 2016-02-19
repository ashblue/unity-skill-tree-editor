using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.SkillTreePro {
	[CustomEditor(typeof(SkillTreeDatabase))]
	public class StatsDataDrawer : Editor {
		public override void OnInspectorGUI () {
			if (GUILayout.Button("Edit Skill Tree")) {
				Wm.Db = serializedObject.targetObject as SkillTreeDatabase;
				Wm.ShowSkillTreeEditor();
			}

			serializedObject.ApplyModifiedProperties();
			DrawDefaultInspector();
			if (GUI.changed) EditorUtility.SetDirty(target);
		}
	}
}
