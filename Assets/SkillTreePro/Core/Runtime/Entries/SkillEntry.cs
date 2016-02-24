using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillEntry {
		public bool unlocked;
		SkillCollectionDefinitionBase definition;
		bool active; // Determines if the skill have been activated or not
		CategoryEntry parentCategory;

		SkillTreeEntry parentSkillTree {
			get {
				return parentCategory.parentSkillTree;
			}
		}

		public bool IsActive {
			get { return active; }
		}

		/// <summary>
		/// Skill pointer always starts at 0
		/// </summary>
		int _skillPointer = 0;

		int SkillPointer {
			get { return _skillPointer; }

			set {
				_skillPointer = Mathf.Clamp(value, 0, definition.skills.Count);
			}
		}

		bool IsSkillMaxed {
			get {
				return SkillPointer + 1 < definition.skills.Count;
			}
		}

		public SkillDefinitionBase Skill {
			get {
				return definition.skills[_skillPointer];
			}
		}

		public SkillEntry (SkillCollectionDefinitionBase def, CategoryEntry cat) {
			definition = def;
			parentCategory = cat;

			Debug.AssertFormat(def.skills.Count < 1, 
				"Skill Collection {0} has no skill entries. Please add some or calling this skill tree will crash", 
				def.DisplayName);
		}

		public void Activate () {
			if (active) {
				Debug.LogErrorFormat("You cannot activate an activate an already active skill. Failed on {0}", 
					definition.DisplayName);
			} else {
				Skill.Activate(parentSkillTree.definition);
				active = true;
			}
		}

		public void Deactive () {
			if (!active) {
				Skill.Deactivate(parentSkillTree.definition);
				active = false;
			} else {
				Debug.LogErrorFormat("You cannot deactivate an inactivate skill. Failed on {0}", 
					definition.DisplayName);
			}
		}

		public bool Hidden {
			get {
				return definition.hidden;
			}
		}

		public bool IsParentUnlocked {
			get {
				return parentCategory.childToParentSkills[definition.uuid].Any(p => unlocked);
				// @TODO We need a child to parent dictionary built inside the skill tree entry
//				return SkillTreeBase.current.GetSkillByUuid();
			}
		}

		// @TODO Also needs to check status
		public bool IsPurchasable {
			get {
				return parentSkillTree.SkillPoints > 0 && !IsSkillMaxed;
			}
		}

		public void Purchase () {
			if (IsPurchasable) {
				// Wipe the existing skill
				if (IsActive) Deactive();

				parentSkillTree.SkillPoints -= 1;
				SkillPointer += 1;

				Activate();
			}
		}
	}
}
