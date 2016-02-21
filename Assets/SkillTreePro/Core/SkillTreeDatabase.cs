using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		[HideInInspector] public int _activeCategoryIndex = -1;
		public SkillCategoryDefinition ActiveCategory {
			get {
				if (_activeCategoryIndex >= 0 && categories.Count > 0) {
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

		public List<string> GetSkillGroupTypes () {
			System.Type[] types = Assembly.GetAssembly(typeof(SkillCollectionDefinitionBase))
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(SkillCollectionDefinitionBase)))
				.ToArray();
				
			return System.Array.ConvertAll(types, x => x.ToString()).ToList();
		}

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
