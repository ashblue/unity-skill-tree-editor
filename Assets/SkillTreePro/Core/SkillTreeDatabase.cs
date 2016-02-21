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
				if (_activeCategoryIndex >= 0 && _activeCategoryIndex < categories.Count) {
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

		public List<string> GetSkillGroupTypes (bool includeHidden = false) {
			List<System.Type> types = Assembly.GetAssembly(typeof(SkillCollectionDefinitionBase))
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(SkillCollectionDefinitionBase)))
				.ToList();

			List<System.Type> removed = new List<System.Type>();
			if (!includeHidden) {
				foreach (System.Type type in types) {
					FieldInfo prop = type.GetField("hideInAddMenu");
					if (prop == null) continue;

					object hidden = prop.GetValue(null);
					if (hidden.Equals(true)) removed.Add(type);
				}
			}
			removed.ForEach(r => types.Remove(r));
				
			return types.ConvertAll(x => x.ToString()).ToList();
		}

		[TextArea(3, 5)]
		public string description;

		public List<SkillCategoryDefinition> categories = new List<SkillCategoryDefinition>();
	}	
}
