using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
