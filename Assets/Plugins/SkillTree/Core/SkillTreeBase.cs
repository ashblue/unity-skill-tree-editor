using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillTreeBase : MonoBehaviour {
		public GraphSidebar[] test;

		public string title = "Skill Tree";
		[TextArea(3, 5)]
		public string description;

		// Used by the skill tree window to discover the currently active category for editing
		[HideInInspector] public SkillCategoryBase currentCategory;

		// Override these base classes with custom classes that inherit from the base
		abstract public System.Type SkillCategory { get; }
		abstract public System.Type SkillCollection { get; }
		abstract public System.Type Skill { get; }

		[Tooltip("How many skill points are available")]
		public int skillPoints = 0; // Number of available skill points

		void Awake () {
			// @TODO Pre-cache all unique IDs for speed purposes
			// @TODO Remove loop methods where possible, not effecient enough

			// Need to get a list of all categories (each with a uuid)
			// dictionary of all category, group, and skill uuids

			// Return a list of all skill groups from a uuid
			// Return a list of all skills from a uuid

			// Check requirements from a skill against current stats
			// Retrieve next unlocked skill from a skill tree
			// Spend point to purchase a skill
		}

		public void SetCategoryLv (string categoryId, int lv) {
			SkillCategoryBase category = GetCategory(categoryId);
			if (category != null) {
				category.skillLv = lv;
			}
		}

		public SkillCategoryBase[] GetCategories () {
			return GetComponentsInChildren<SkillCategoryBase>();
		}

		public SkillCategoryBase GetCategory (string categoryId) {
			SkillCategoryBase category = null;
			foreach (Transform child in transform) {
				SkillCategoryBase cat = child.GetComponent<SkillCategoryBase>();
				if (cat != null && cat.uniqueName == categoryId) {
					category = cat;
					break;
				}
			}

			return category;
		}

		public SkillCollectionBase GetCollection (string categoryId, string collectionId) {
			SkillCollectionBase collection = null;
			SkillCategoryBase category = GetCategory(categoryId);

			if (category != null) {
				foreach (Transform child in category.transform) {
					SkillCollectionBase col = child.GetComponent<SkillCollectionBase>();
					if (col != null && col.uniqueName == collectionId) {
						collection = col;
						break;
					}
				}
			}
			
			return collection;
		}

		public SkillBase GetSkill (string categoryId, string collectionId, string skillId) {
			SkillCollectionBase collection = GetCollection(categoryId, collectionId);
			SkillBase skill = null;

			if (collection != null) {
				foreach (Transform child in collection.transform) {
					SkillBase s = child.GetComponent<SkillBase>();
					if (s != null && s.uniqueName == skillId) {
						skill = s;
						break;
					}
				}
			}
			
			return skill;
		}

		// @TODO Should spit back a giant array of data so the user can save it however they want
		// @TODO change to abstract when moving SkillTree to an abstract class
		virtual public void Save () {
//			Dictionary<string, bool> skillRecords = new Dictionary<string, bool>();
//
//			foreach (Transform collection in transform) {
//				foreach (Transform child in collection.transform) {
//					Skill skill = child.GetComponent<Skill>();
//					if (string.IsNullOrEmpty(skill.uuid)) {
//						Debug.LogErrorFormat("Skill {0} has no UUID", skill.uuid);
//					} else {
//						skillRecords.Add(skill.uuid, skill.unlocked);
//					}
//				}
//			}

			// Create a special save class that we can load or create
//			SaveData saveData = new SaveData {
//				name = title,
//				skills = skillRecords
//			};

			// Turn save class into JSON via JSON.net or your preferred JSON library
		}

		// @TODO change to abstract when moving SkillTree to an abstract class
		// @TODO Should swallow Save data format to update all active skills
		virtual public void Load () {
			// Loop through skill dictionary
			// For each UUID apply the unlocked skill value
			// If we hit a missing UUID fire a warning
		}
	}
}
