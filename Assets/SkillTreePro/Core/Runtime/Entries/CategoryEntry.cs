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
			this.definition = definition;
			this.parentSkillTree = parentSkillTree;
			definition.skillCollections.ForEach(col => BuildSkill(col));
		}

		void BuildSkill (SkillCollectionDefinitionBase def) {
			SkillEntry s = new SkillEntry(def, this);
			s.unlocked = def is SkillCollectionStartDefinition; // Start nodes are always automatically unlocked
			skills.Add(s);
			skillsById[def.id] = s;
			skillsByUuid[def.uuid] = s;

			def.childCollections.ForEach(c => childToParentSkills[c.uuid].Add(s));
		}

		void Requirements () {
			// @TODO Check if this category is unlocked by querying the definitions base requirements
		}
	}
}
