using UnityEngine;
using UnityEditor;

namespace Adnc.SkillTreePro {
	public class NodeGraphWindow : EditorWindow {
		public readonly float sidebarWidth = 240f; // Size of the sidebar
		NodeGraphSidebar sidebar = new NodeGraphSidebar();
		NodeGraphMain main = new NodeGraphMain();

		void OnEnable () {
			Wm.Win = this;
		}

		void OnGUI () {
			if (Wm.Db == null) {
				EditorGUILayout.LabelField("Please select a skill tree database to begin editing.");
				return;
			}

			main.Update(new Rect(0f, 0f, position.width - sidebarWidth, position.height));
			sidebar.Update(new Rect(position.width - sidebarWidth, 0, sidebarWidth, position.height));
		}
	}
}
