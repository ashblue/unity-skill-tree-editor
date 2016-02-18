using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCategoryDefinition {
		[Tooltip("Display name for the category")]
		public string displayName = "Category";

		[Tooltip("Used for data retrieval purposes")]
		public string id;

		[Tooltip("Optional index used to determine the category placement")]
		public int sortIndex;

		[Tooltip("What is the starting skill level in this category?")]
		public int defaultSkillLv = 0;

		[Tooltip("UUID generated at creation")]
		[HideInInspector] public string uuid;

		public List<SkillCollectionDefinition> collections = new List<SkillCollectionDefinition>();

		[TextArea(3, 5)]
		public string description;

		[TextArea(3, 5)]
		public string notes;

		public SkillCategoryDefinition () {
			uuid = System.Guid.NewGuid().ToString();
		}
	}
}
