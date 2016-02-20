using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		[HideInInspector] public int _activeCategoryIndex = -1;
		public SkillCategoryDefinition ActiveCategory {
			get {
				if (_activeCategoryIndex >= 0) {
					return categories[_activeCategoryIndex];
				}

				return null;
			}

			set {
				if (value == null) {
					_activeCategoryIndex = -1;
				} else {
					_activeCategoryIndex = categories.FindIndex(a => a == value);
				}
			}
		}

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
