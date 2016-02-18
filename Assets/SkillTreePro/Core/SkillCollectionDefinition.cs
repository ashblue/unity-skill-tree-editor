using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCollectionDefinition {
		public NodeData node;

		[Tooltip("Name the user will see when the node is printed")]
		public string displayName = "Collection";

		[Tooltip("Used for data lookup purposes")]
		public string id;

		[Tooltip("Determines what skill is currently active and returns it")]
		public int skillIndex = 0;

		[Tooltip("UUID generated at creation")]
		[HideInInspector] public string uuid;

		// public List<SkillCollectionBase> childSkills = new List<SkillCollectionBase>();

		[TextArea(3, 5)]
		public string description;

		[TextArea(3, 5)]
		public string notes;

		public SkillCollectionDefinition () {
			uuid = System.Guid.NewGuid().ToString();
		}
	}
}
