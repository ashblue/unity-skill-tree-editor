using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class CategoryEntry {
		public SkillTreeEntry parentSkillTree;
		public SkillCategoryDefinitionBase definition;
		public List<SkillEntry> skills = new List<SkillEntry>();
		public Dictionary<string, SkillEntry> skillsById = new Dictionary<string, SkillEntry>();
		public Dictionary<string, SkillEntry> skillsByUuid = new Dictionary<string, SkillEntry>();
		public Dictionary<string, List<SkillEntry>> childToParentSkills = new Dictionary<string, List<SkillEntry>>();

		int _currentLevel;
		int CurrentLevel {
			get { return _currentLevel; }

			set {
				_currentLevel = Mathf.Max(value, 0);
			}
		}

		public CategoryEntry (SkillCategoryDefinitionBase definition, SkillTreeEntry parentSkillTree) {
			if (SkillTreeBase.current.debug) Debug.LogFormat("Parsing category: {0}", definition.DisplayName);

			this.definition = definition;
			this.parentSkillTree = parentSkillTree;
			definition.skillCollections.ForEach(col => BuildSkill(col));
		
			if (SkillTreeBase.current.debug) Debug.LogFormat("Category '{0}' successfully generated", definition.DisplayName);
		}

		void BuildSkill (SkillCollectionDefinitionBase def) {
			SkillEntry s = new SkillEntry(def, this);
			s.unlocked = def is SkillCollectionStartDefinition; // Start nodes are always automatically unlocked
			skills.Add(s);
			skillsById[def.id] = s;
			skillsByUuid[def.uuid] = s;

			foreach (SkillCollectionDefinitionBase c in def.childCollections) {
				if (!childToParentSkills.ContainsKey(c.uuid)) {
					childToParentSkills[c.uuid] = new List<SkillEntry>();
				}

				childToParentSkills[c.uuid].Add(s);
			}
		}
	}
}
