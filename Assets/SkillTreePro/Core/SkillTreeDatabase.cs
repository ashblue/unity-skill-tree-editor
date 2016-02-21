using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Adnc.SkillTreePro {
	public class SkillTreeDatabase : ScriptableObject {
		public string title = "Untitled";

		[TextArea(3, 5)]
		public string description;

		[HideInInspector] public List<SkillCategoryDefinitionBase> categories = new List<SkillCategoryDefinitionBase>();

		[HideInInspector] public int _activeCategoryIndex = -1;
		public SkillCategoryDefinitionBase ActiveCategory {
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

		public List<string> GetInheritedTypes (System.Type parentType, bool includeHidden) {
			List<System.Type> types = Assembly.GetAssembly(parentType)
				.GetTypes()
				.Where(t => t.IsSubclassOf(parentType))
				.ToList();

			List<System.Type> removed = new List<System.Type>();

			// Reject all classes with a static property "hideInAddMenu"
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

		public List<string> GetSkillCategoryTypes (bool includeHidden = false) {
			return GetInheritedTypes(typeof(SkillCategoryDefinitionBase), includeHidden);
		}

		public List<string> GetSkillCollectionTypes (bool includeHidden = false) {
			return GetInheritedTypes(typeof(SkillCollectionDefinitionBase), includeHidden);
		}
	}	
}
