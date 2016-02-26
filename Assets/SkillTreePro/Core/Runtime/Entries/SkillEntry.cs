using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillEntry {
		public bool unlocked;
		SkillCollectionDefinitionBase definition;
		public bool active = true; // Determines if the skill have been activated or not
		CategoryEntry parentCategory;

		SkillTreeEntry parentSkillTree {
			get {
				return parentCategory.parentSkillTree;
			}
		}

		public bool IsLevelRequirement {
			get {
				return parentCategory.CurrentLevel >= Skill.requiredLevel;
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
			if (SkillTreeBase.current.debug) Debug.LogFormat("Parsing skill: {0}", def.DisplayName);

			definition = def;
			parentCategory = cat;

			// Allow parents will prevent start nodes from being picked up
			Debug.AssertFormat(def.skills.Count > 0, 
				"Skill Collection '{0}' has {1} skill entries. Please add some or calling this skill tree will crash", 
				def.DisplayName, def.skills.Count);

			// Activate defaults
			SkillPointer = definition.skillIndex;
			unlocked = Skill.unlocked;

			if (SkillPointer > 0) {
				Debug.AssertFormat(IsParentUnlocked, 
					"Parent is locked for skill collection {0}, should not be setting the active skill to anything greater than 0", 
					definition.DisplayName);
			}

			if (unlocked) {
				Debug.AssertFormat(IsParentUnlocked, 
					"Parent is locked for skill collection {0}, should not be unlocking child by default", 
					definition.DisplayName);

				Debug.AssertFormat(parentCategory.CurrentLevel >= Skill.requiredLevel,
					"Skill collection '{0}' category level is {1} but requires {2}, should not be unlocking this by default", 
					definition.DisplayName, parentCategory.CurrentLevel, Skill.requiredLevel);
			}

			if (SkillTreeBase.current.debug) Debug.LogFormat("Skill '{0}' successfully generated", def.DisplayName);
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
			}
		}

		public bool IsPurchasable {
			get {
				bool canPurchase = parentSkillTree.SkillPoints > 0;
				return canPurchase && IsParentUnlocked && !Hidden && !IsSkillMaxed && IsLevelRequirement;
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

		public NodeStatus GetStatus () {
			if (Hidden) {
				return NodeStatus.Hidden;
			} else if (IsPurchasable) {
				return NodeStatus.Purchasable;
			}

			return NodeStatus.Locked;
		}
	}
}
