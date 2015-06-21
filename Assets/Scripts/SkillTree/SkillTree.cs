using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree {
	public class SkillTree : MonoBehaviour {
		public GraphSidebar[] test;

		public string title = "Skill Tree";
		[TextArea(3, 5)]
		public string description;

		// Used by the skill tree window to discover the currently active category for editing
		[HideInInspector] public SkillCategory currentCategory;
	}
}
