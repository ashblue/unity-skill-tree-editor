using UnityEngine;
using UnityEditor;

namespace Adnc.SkillTreePro {
	public class NodeGraphWindow : EditorWindow {
		float sidebarWidth = 240f; // Size of the sidebar
		NodeGraphSidebar sidebar = new NodeGraphSidebar();

		void OnGUI () {
			if (Wm.Db == null) {
				EditorGUILayout.LabelField("Please select a skill tree database to begin editing.");
				return;
			}

			sidebar.Update(new Rect(position.width - sidebarWidth, 0, sidebarWidth, position.height));
		}
	}
}
