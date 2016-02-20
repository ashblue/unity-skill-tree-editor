using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		/// <summary>
		/// Currently selected category for editing
		/// </summary>
		public SkillCategoryDefinition _eCat;

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
