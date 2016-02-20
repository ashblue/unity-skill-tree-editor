using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		/// <summary>
		/// Currently selected category for editing
		/// </summary>
		[HideInInspector] public int _activeCategoryIndex = -1;
		public SkillCategoryDefinition ActiveCategory {
			get {
				if (_activeCategoryIndex >= 0) {
					return categories[_activeCategoryIndex];
				}

				return null;
			}
		}

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
